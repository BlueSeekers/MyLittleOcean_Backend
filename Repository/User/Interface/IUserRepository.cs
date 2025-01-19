public interface IUserRepository {
    Task<bool> AddUserAsync(string userId, string userName, string provider);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<int> CreateUserAsync(AuthCreateDto userCreateDto);
    Task<int> CreateDefaultUserDataAsync(int userNo);
}