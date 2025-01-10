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
    public UserFullData? GetUserFullData(string id) {
        UserFullData userFullData = new UserFullData();
        
        userFullData.UserInfo = _userInfoRepository.getUserInfo(id);
        //TODO: NULL 예외처리

        userFullData.UserData = _userDataRepository.GetUserData(id);
        //TODO: NULL 예외처리

        return userFullData;
    }

    /// <summary>
    /// 유저 전체 정보 UserNo로 조회
    /// </summary>
    /// <param name="no">UserNo</param>
    /// <returns>UserFullData<UserInfo, UserData></returns>
    public UserFullData? GetUserFullData(int no) {
        UserFullData userFullData = new UserFullData();
        userFullData.UserInfo = _userInfoRepository.getUserInfo(no);
        //TODO: NULL 예외처리
        userFullData.UserData = _userDataRepository.GetUserData(no);
        //TODO: NULL 예외처리

        return userFullData;
    }
}
