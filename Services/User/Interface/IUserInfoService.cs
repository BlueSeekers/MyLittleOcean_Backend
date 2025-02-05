public interface IUserInfoService {

    Task<ServiceResult<UserFullData>> GetUserFullDataById(string id);

    Task<ServiceResult<UserFullData>> GetUserFullDataByNo(int userNo);

    Task<bool> UpdateUserNameAsync(string userId, string newUserName);
}
