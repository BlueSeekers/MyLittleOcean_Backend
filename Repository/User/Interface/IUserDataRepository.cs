public interface IUserDataRepository {

    //UserId로 유저 데이터 조회
    UserData GetUserDataById(string userid);

    //UserNo로 유저 데이터 조회
    UserData GetUserDataByNo(int userNo);

    //UserNo로 코인 사용
    int UseCoinByNo(UseDataDto useDataDto);

    //UserId로 코인 사용
    int UseCoinByID(UseDataDto useByIdDto);

    //UserNo로 토큰 사용
    int UseTokenByNo(UseDataDto useDataDto);

    //UserId로 토큰 사용
    int UseTokenByID(UseDataDto useByIdDto);
}
