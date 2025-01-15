using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase {
    private readonly string _jwtKey = "blueseekers_0703_my_little_ocean_story"; // JWT 비밀 키
    private readonly string _issuer = "http://localhost:7122";
    private readonly string _audience = "http://localhost:7122";

    //private readonly IAuthService _userService;

    //public AuthController(IAuthService userService) {
    //    _userService = userService;
    //}              
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequestDto request) {
        if (request.Username == "test" && request.Password == "password") {
            // Access Token 생성
            var accessToken = GenerateToken(request.Username, TimeSpan.FromMinutes(30)); // 유효기간 30분

            // Refresh Token 생성
            var refreshToken = GenerateToken(request.Username, TimeSpan.FromDays(7)); // 유효기간 7일

            return Ok(new {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        return Unauthorized("Invalid username or password.");
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequestDto request) {
        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);

            // Refresh Token 검증
            var principal = tokenHandler.ValidateToken(request.RefreshToken, new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true, // 만료 여부 검증
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            // Refresh Token이 유효하면 새로운 Access Token 생성
            var username = principal.Identity?.Name; // 사용자 이름 가져오기
            var newAccessToken = GenerateToken(username, TimeSpan.FromMinutes(30)); // 새로운 Access Token 발급 - 60분

            return Ok(new {
                AccessToken = newAccessToken
            });
        }
        catch (SecurityTokenExpiredException) {
            return Unauthorized("Refresh token has expired.");
        }
        catch (Exception ex) {
            return Unauthorized($"Invalid refresh token: {ex.Message}");
        }
    }


    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] SocialLoginRequest request)
    {
        try
        {
            var payload = await ValidateGoogleToken(request.IdToken);

            // DB에 저장
            var saved = await _userRepository.AddUserAsync(
                payload.Subject,      // UserId
                payload.Name,         // UserName
                "google"            // Provider
            );

            if (!saved)
            {
                return StatusCode(500, "Failed to save user");
            }

            // 토큰 생성
            var accessToken = GenerateToken(payload.Name, TimeSpan.FromMinutes(30));
            return Ok(new { AccessToken = accessToken });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpPost("gpgs-login")]
    public async Task<IActionResult> GpgsLogin([FromBody] SocialLoginRequest request)
    {
        try
        {
            var payload = await ValidateGpgsToken(request.IdToken);

            // DB에 저장
            var saved = await _userRepository.AddUserAsync(
                payload.PlayerId,      // UserId
                payload.Name,         // UserName
                "google_play_games"   // Provider
            );

            if (!saved)
            {
                return StatusCode(500, "Failed to save user");
            }

            // 토큰 생성
            var accessToken = GenerateToken(payload.Name, TimeSpan.FromMinutes(30));
            return Ok(new { AccessToken = accessToken });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
    {
        // 테스트용
        //return new GoogleJsonWebSignature.Payload
        //{
        //    Email = "test@example.com",
        //    Name = "Test User" + idToken,
        //    Subject = "test123" + idToken // Google의 고유 ID 역할
        //};
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { _configuration["Authentication:Google:ClientId"] }
        };

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (InvalidJwtException)
        {
            throw new Exception("Invalid Google token.");
        }
    }
    private async Task<PlayGamesPayload> ValidateGpgsToken(string idToken)
    {
        using (var client = new HttpClient())
        {
            // GPGS는 다른 엔드포인트 사용
            var url = $"https://www.googleapis.com/games/v1/applications/{_configuration["Authentication:GPGS:AppId"]}/verify/";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Invalid GPGS token");
            }

            var content = await response.Content.ReadFromJsonAsync<PlayGamesPayload>();
            return content;
        }
    }
    private string GenerateToken(string username, TimeSpan validFor) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.Add(validFor), // 유효기간 설정
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    //[HttpPost("signup")]
    //public IActionResult CreateUser([FromBody] AuthCreateDto userCreateDto) {
    //    if (userCreateDto.userId.IsNullOrEmpty()) {
    //        return BadRequest("No information exists");
    //    }
    //    try {
    //        int createUser = _userService.CreateUser(userCreateDto);
    //        if (createUser > 0)
    //            return Ok(new { message = "User created successfully." });
    //        else
    //            return StatusCode(500, new { message = "Failed to create user." });
    //    }
    //    catch (Exception e) {
    //        return BadRequest(new { error = e.Message });
    //    }
    //}

}
