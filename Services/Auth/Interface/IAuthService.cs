public interface IAuthService {
    Task<int> CreateUserAsync(AuthCreateDto userCreateDto);
    Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password);
    Task<string> RefreshTokenAsync(string refreshToken);
    Task<(string AccessToken, string RefreshToken)> GoogleLoginAsync(string idToken);
    Task<(string AccessToken, string RefreshToken)> GpgsLoginAsync(string idToken);
    Task<bool> UpdateUserNameAsync(int userNo, string newUserName);
    Task<bool> UpdateUserNameAsync(string userId, string newUserName);
}
