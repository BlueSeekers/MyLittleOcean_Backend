public interface IUserRepository
{
    Task<bool> AddUserAsync(string userId, string userName, string email, string provider, string providerId);
}