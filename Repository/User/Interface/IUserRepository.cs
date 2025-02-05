public interface IUserRepository {
    Task<bool> AddUserAsync(string userId, string userEmail, string provider);
    Task<bool> ValidateUserCredentialsAsync(string userId);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<int> CreateUserAsync(AuthCreateDto userCreateDto);
    Task<int> CreateDefaultUserDataAsync(int userNo);
}