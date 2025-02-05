public interface IUserDataService {

    //UserNo로 코인 사용
    Task<ServiceResult<int>> UseCoinByNo(UserDataDto useDataDto);

    //UserID로 코인 사용
    Task<ServiceResult<int>> UseCoinByID(UserDataDto useDataDto);

    //UserNo로 토큰 사용
    Task<ServiceResult<int>> UseTokenByNo(UserDataDto useDataDto);

    //UserID로 토큰 사용
    Task<ServiceResult<int>> UseTokenByID(UserDataDto useDataDto);
}
