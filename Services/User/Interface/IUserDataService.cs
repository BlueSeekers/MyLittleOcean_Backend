public interface IUserDataService {

    //UserNo로 코인 사용
    int UseTokenByNo(UseDataDto useDataDto);

    //UserID로 코인 사용
    int UseCoinByID(UseDataDto useDataDto);

    //UserNo로 토큰 사용
    int UsertTokenByNo(UseDataDto useDataDto);

    //UserID로 토큰 사용
    int UseTokenByID(UseDataDto useDataDto);
}
