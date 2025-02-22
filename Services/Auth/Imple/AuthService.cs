﻿using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService {
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _jwtKey;
    private readonly string _issuer;
    private readonly string _audience;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, HttpClient httpClient) {
        _userRepository = userRepository;
        _configuration = configuration;
        _httpClient = httpClient;
        _jwtKey = configuration["JwtSettings:Secret"];
        _issuer = configuration["JwtSettings:Issuer"];
        _audience = configuration["JwtSettings:Audience"];
    }

    public async Task<int> CreateUserAsync(AuthCreateDto userCreateDto) {
        if (string.IsNullOrEmpty(userCreateDto.userId)) {
            throw new ArgumentException("User ID is required");
        }

        // UserRepository를 통해 사용자 생성
        var userNo = await _userRepository.CreateUserAsync(userCreateDto);
        if (userNo <= 0) {
            throw new Exception("Failed to create user");
        }

        // 기본 사용자 데이터 생성
        var defaultDataResult = await _userRepository.CreateDefaultUserDataAsync(userNo);
        if (defaultDataResult <= 0) {
            throw new Exception("Failed to create default user data");
        }

        return userNo;
    }

    public async Task<(string AccessToken, string RefreshToken)> LoginAsync(string userId) {
        var isValid = await _userRepository.ValidateUserCredentialsAsync(userId);
        if (!isValid) {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var accessToken = GenerateToken(userId, TimeSpan.FromDays(30));
        var refreshToken = GenerateToken(userId, TimeSpan.FromDays(30));

        return (accessToken, refreshToken);
    }

    public async Task<string> RefreshTokenAsync(string refreshToken) {
        var principal = ValidateToken(refreshToken);
        var username = principal.Identity?.Name;
        if (string.IsNullOrEmpty(username)) {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        return GenerateToken(username, TimeSpan.FromMinutes(30));
    }

    public async Task<(string AccessToken, string RefreshToken)> GoogleLoginAsync(string idToken) {
        var payload = await ValidateGoogleToken(idToken);
        await SaveUserAsync(payload.Subject, payload.Email, payload.Picture, "google");

        var accessToken = GenerateToken(payload.Email, TimeSpan.FromMinutes(30));
        var refreshToken = GenerateToken(payload.Email, TimeSpan.FromDays(7));

        return (accessToken, refreshToken);
    }

    private async Task SaveUserAsync(string userId, string userEmail, string userImage, string provider) {
        var saved = await _userRepository.AddUserAsync(userId, userEmail, userImage, provider);
        if (!saved) {
            throw new Exception("Failed to save user");
        }
    }

    private string GenerateToken(string username, TimeSpan validFor) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.Add(validFor),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Token validation and other helper methods moved from controller
    private ClaimsPrincipal ValidateToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        try {
            return tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);
        }
        catch (Exception ex) {
            throw new UnauthorizedAccessException($"Invalid token: {ex.Message}");
        }
    }

    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken) {
        if (string.IsNullOrEmpty(idToken)) {
            throw new ArgumentException("Token cannot be null or empty", nameof(idToken));
        }

        var settings = new GoogleJsonWebSignature.ValidationSettings() {
            Audience = new List<string> { _configuration["Authentication:Google:ClientId"] }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        if (payload == null) {
            throw new UnauthorizedAccessException("Invalid Google token");
        }

        return payload;
    }
}
