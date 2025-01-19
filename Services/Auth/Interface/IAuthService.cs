public interface IAuthService {
    Task<int> CreateUserAsync(AuthCreateDto userCreateDto);
    Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password);
    Task<string> RefreshTokenAsync(string refreshToken);
    Task<string> GoogleLoginAsync(string idToken);
    Task<string> GpgsLoginAsync(string idToken);
}
