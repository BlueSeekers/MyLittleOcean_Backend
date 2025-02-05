
public interface IUserInfoRepository {
    Task<UserInfo?> GetUserInfoById(string id);
    Task<UserInfo?> GetUserInfoByNo(int no);

    Task<int> UpdateUserNameAsync(string usrId, string userName);
}
