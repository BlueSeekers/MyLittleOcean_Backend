public interface IUserDataRepository {

    UserData GetUserData(string userid);
    UserData GetUserData(int userNo);
}
