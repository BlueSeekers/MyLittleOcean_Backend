public interface IUserDataService {

    //UserNo로 코인 사용
    Task<ServiceResult<int>> UseCoinByNo(UserUseDto useDataDto);

    //UserID로 코인 사용
    Task<ServiceResult<int>> UseCoinByID(UserUseDto useDataDto);

    //UserNo로 토큰 사용
    Task<ServiceResult<int>> UseTokenByNo(UserUseDto useDataDto);

    //UserID로 토큰 사용
    Task<ServiceResult<int>> UseTokenByID(UserUseDto useDataDto);

    //Data Update
    Task<ServiceResult<UserData>> UserDataUpdate(UserUpdateDataDto userUpdateDto);
}
