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
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request) {
        try {
            var (accessToken, refreshToken) = await _authService.LoginAsync(request.Username, request.Password);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }
        catch (UnauthorizedAccessException) {
            return Unauthorized("Invalid username or password.");
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request) {
        try {
            var newAccessToken = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(new { AccessToken = newAccessToken });
        }
        catch (UnauthorizedAccessException ex) {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("google/login")]
    public async Task<IActionResult> GoogleLogin([FromBody] SocialLoginRequest request) {
        try {
            var (accessToken, refreshToken) = await _authService.GoogleLoginAsync(request.IdToken);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }
        catch (ArgumentException ex) {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex) {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception) {
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    [HttpPost("gpgs/login")]
    public async Task<IActionResult> GpgsLogin([FromBody] SocialLoginRequest request) {
        try {
            var (accessToken, refreshToken) = await _authService.GpgsLoginAsync(request.IdToken);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }
        catch (ArgumentException ex) {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex) {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception) {
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> CreateUser([FromBody] AuthCreateDto userCreateDto) {
        if (string.IsNullOrEmpty(userCreateDto.userId)) {
            return BadRequest(new { error = "No information exists" });
        }

        try {
            var userNo = await _authService.CreateUserAsync(userCreateDto);
            return Ok(new { message = "User created successfully" });
        }
        catch (ArgumentException ex) {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex) {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
