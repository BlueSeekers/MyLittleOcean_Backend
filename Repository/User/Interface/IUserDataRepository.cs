public interface IUserDataRepository {

    //UserId로 유저 데이터 조회
    Task<UserData?> GetUserDataById(string userid);

    //UserNo로 유저 데이터 조회
    Task<UserData?> GetUserDataByNo(int userNo);

    //UserNo로 코인 사용
    Task<int> UseCoinByNo(UserDataDto useDataDto);

    //UserId로 코인 사용
    Task<int> UseCoinByID(UserDataDto useByIdDto);

    //UserNo로 토큰 사용
    Task<int> UseTokenByNo(UserDataDto useDataDto);

    //UserId로 토큰 사용
    Task<int> UseTokenByID(UserDataDto useByIdDto);
}
