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
    public UserFullData? GetUserFullDataById(string id) {
        UserFullData userFullData = new UserFullData();

        UserInfo info = new UserInfo();
        UserData data = new UserData();
        info = _userInfoRepository.GetUserInfoById(id);
        if (info != null) {
            data = _userDataRepository.GetUserDataById(id);
            if (data != null) {
                userFullData.UserInfo = info;
                userFullData.UserData = data;   
            }
        }

        return userFullData;
    }

    /// <summary>
    /// 유저 전체 정보 UserNo로 조회
    /// </summary>
    /// <param name="no">UserNo</param>
    /// <returns>UserFullData<UserInfo, UserData></returns>
    public UserFullData? GetUserFullDataByNo(int no) {
        UserFullData userFullData = new UserFullData();

        UserInfo info = new UserInfo();
        UserData data = new UserData();

        info = _userInfoRepository.GetUserInfoByNo(no);
        if (info != null) {
            data = _userDataRepository.GetUserDataByNo(no);
            if (data != null){
                userFullData.UserInfo = info;
                userFullData.UserData = data;
            }
        }
        return userFullData;
    }

}
