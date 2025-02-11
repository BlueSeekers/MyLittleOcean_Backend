using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 환경 변수 및 설정 값 확인
var jwtKey = builder.Configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JWT Secret is not configured.");
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JWT Issuer is not configured.");
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JWT Audience is not configured.");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string is not configured.");

// 서비스 등록
ConfigureServices(builder.Services, jwtKey, jwtIssuer, jwtAudience, connectionString);

// 애플리케이션 빌드
var app = builder.Build();

// 미들웨어 구성
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, string jwtKey, string jwtIssuer, string jwtAudience, string connectionString) {
    // QueryLogger 등록
    services.AddScoped<QueryLogger>();
    // Dapper Extensions 추가 (snake to camel)
    DapperExtensions.UseSnakeCaseToCamelCaseMapping();

    // 컨트롤러 및 글로벌 경로 프리픽스 설정
    services.AddControllers(options => {
        options.Conventions.Add(new GlobalRoutePrefix("api/v1"));
    })
    .AddJsonOptions( options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // MySQL DB 연결 설정
    services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));

    // MySQL용 DB 연결 설정
    services.AddSingleton<LoggingService>();
    services.AddHostedService<UdpServerService>();

    // Swagger(OpenAPI) 설정
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
            Title = "MyLIO API",
            Version = "v1",
            Description = "API documentation for MyLIO.",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact {
                Name = "Support Team",
                Email = "support@mylio.com",
                Url = new Uri("https://mylio.com/contact")
            }
        });

        // JWT 인증 설정 추가
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter 'Bearer' followed by your token in the text box below.\nExample: Bearer <AccessToken>"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        c.SchemaFilter<EnumSchemaFilter>();
        c.EnableAnnotations();
    });

    // JWT 인증 설정
    var key = Encoding.UTF8.GetBytes(jwtKey);
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            options.Events = new JwtBearerEvents {
                OnChallenge = async context => {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"error\": \"Token is invalid or missing\"}");
                }
            };
        });

    services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));

    // MySQL용 DB 연결 설정
    services.AddSingleton<LoggingService>();
    services.AddHostedService<UdpServerService>();

    // 기타 서비스 등록
    services.AddAuthModule(builder.Configuration);               //Auth
    services.AddFollowModule(connectionString);             //Fllow
    services.AddRankingModule(connectionString);            //Rank
    services.AddUserModule(connectionString);               //User

    // CORS 설정
    services.AddCors(options => {
        options.AddDefaultPolicy(policy => {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    // 라우팅 설정
    services.Configure<RouteOptions>(options => {
        options.LowercaseUrls = true;
        options.AppendTrailingSlash = false;
    });
}

void ConfigureMiddleware(WebApplication app) {
    // 사용자 정의 에러 핸들링 미들웨어 추가
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<LoggingMiddleware>();

    // 개발 환경에서 Swagger 활성화
    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI(c => {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyLIO API V1");
            c.RoutePrefix = string.Empty; // Swagger UI
        });
    }

    // CORS 활성화
    app.UseCors();

    // 인증 및 권한 부여
    app.UseAuthentication();
    app.UseAuthorization();

    // 컨트롤러 매핑
    app.MapControllers();

    app.MapGet("/", () => "Hello World!");

    app.Run("http://0.0.0.0:5000");
}

public class EnumSchemaFilter : ISchemaFilter {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context) {
        if (context.Type.IsEnum) {
            schema.Enum.Clear();
            foreach (var name in Enum.GetNames(context.Type)) {
                schema.Enum.Add(new OpenApiString(name));
            }
        }
    }
}