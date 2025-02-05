public class UserInfoService : IUserInfoService {
    private readonly IUserInfoRepository _userInfoRepository;
    private readonly IUserDataRepository _userDataRepository;

    public UserInfoService(
        IUserInfoRepository userInfoRepository,
        IUserDataRepository userDataRepository) {
        _userInfoRepository = userInfoRepository;
        _userDataRepository = userDataRepository;
    }

    /// <summary>
    /// 유저 전체 정보 ID로 조회
    /// </summary>
    /// <param name="id">UserId</param>
    /// <returns>UserFullData<UserInfo, UserData></returns>
    public async Task<ServiceResult<UserFullData>> GetUserFullDataById(string id) {
        UserFullData userFullData = new UserFullData();

        UserInfo? info = await _userInfoRepository.GetUserInfoById(id);
        if (info == null) {
            return new ServiceResult<UserFullData>(false, "Not Exist User Info");
        }

        UserData? data = await _userDataRepository.GetUserDataById(id);
        if (data == null) {
            return new ServiceResult<UserFullData>(false, "Not Exist User Data");
        }

        userFullData.UserInfo = info;
        userFullData.UserData = data;

        return new ServiceResult<UserFullData>(true, "Success", userFullData);
    }

    /// <summary>
    /// 유저 전체 정보 UserNo로 조회
    /// </summary>
    /// <param name="no">UserNo</param>
    /// <returns>UserFullData<UserInfo, UserData></returns>
    public async Task<ServiceResult<UserFullData>> GetUserFullDataByNo(int no) {
        UserFullData userFullData = new UserFullData();

        UserInfo? info = await _userInfoRepository.GetUserInfoByNo(no);
        if (info == null) {
            return new ServiceResult<UserFullData>(false, "Not Exist User Info");
        }

        UserData? data = await _userDataRepository.GetUserDataByNo(no);
        if (data == null) {
            return new ServiceResult<UserFullData>(false, "Not Exist User Data");
        }

        userFullData.UserInfo = info;
        userFullData.UserData = data;

        return new ServiceResult<UserFullData>(true, "Success", userFullData);
    }

    public async Task<bool> UpdateUserNameAsync(string userId, string newUserName) {
        if (string.IsNullOrEmpty(newUserName)) {
            throw new ArgumentException("User name is required");
        }

        var updateResult = await _userInfoRepository.UpdateUserNameAsync(userId, newUserName);
        if (updateResult <= 0) {
            throw new Exception("Failed to update user name. The name might be duplicate or user doesn't exist");
        }

        return true;
    }
}
