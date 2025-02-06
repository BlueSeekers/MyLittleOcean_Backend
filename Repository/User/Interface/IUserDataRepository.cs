public interface IUserDataRepository {

    //UserId로 유저 데이터 조회
    Task<UserData?> GetUserDataById(string userid);

    //UserNo로 유저 데이터 조회
    Task<UserData?> GetUserDataByNo(int userNo);

    //UserNo로 코인 사용
    Task<int> UseCoinByNo(UserUseDto useDataDto);

    //UserId로 코인 사용
    Task<int> UseCoinByID(UserUseDto useByIdDto);

    //UserNo로 토큰 사용
    Task<int> UseTokenByNo(UserUseDto useDataDto);

    //UserId로 토큰 사용
    Task<int> UseTokenByID(UserUseDto useByIdDto);

    //유저 게임 데이터 업데이트
    Task<bool> UserDataUpdate(UserUpdateDataDto userUpdateDataDto);
}
