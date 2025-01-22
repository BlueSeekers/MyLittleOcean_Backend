using Microsoft.Extensions.Configuration;

public static class AuthModule {

    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration) {

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        // Repository 등록
        services.AddScoped<IUserRepository>(provider => new UserRepository(connectionString));

        // HttpClient 등록 (소셜 로그인에 필요)
        services.AddHttpClient();

        // Service 등록
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}