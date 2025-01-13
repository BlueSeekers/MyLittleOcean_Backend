public interface IUserInfoService {

    UserFullData GetUserFullDataById(string id);

    UserFullData GetUserFullDataByNo(int userNo);
}
