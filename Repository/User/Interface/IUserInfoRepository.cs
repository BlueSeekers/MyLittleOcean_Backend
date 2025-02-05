
public interface IUserInfoRepository {
    Task<UserInfo?> GetUserInfoById(string id);
    Task<UserInfo?> GetUserInfoByNo(int no);

    Task<bool> IsNameDuplicate(string name);

    Task<bool> UpdateUserName(string userId, string name);
}
