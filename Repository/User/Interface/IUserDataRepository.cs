public interface IUserDataRepository {

    UserData GetUserDataById(string userid);
    UserData GetUserDataByNo(int userNo);
}
