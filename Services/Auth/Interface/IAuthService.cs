public interface IAuthService {
    Task<int> CreateUserAsync(AuthCreateDto userCreateDto);
    Task<(string AccessToken, string RefreshToken)> LoginAsync(string userId);
    Task<string> RefreshTokenAsync(string refreshToken);
    Task<(string AccessToken, string RefreshToken)> GoogleLoginAsync(string idToken);
    Task<(string AccessToken, string RefreshToken)> GpgsLoginAsync(string idToken);
}
