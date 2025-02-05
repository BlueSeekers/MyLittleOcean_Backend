public interface IUserInfoService {

    Task<ServiceResult<UserFullData>> GetUserFullDataById(string id);

    Task<ServiceResult<UserFullData>> GetUserFullDataByNo(int userNo);

    Task<ServiceResult<UserFullData>> UpdateUserName(UserNameUpdateDto userNameUpdateDto);
}
