
public class UserDataRepository : IUserDataRepository {
    private readonly string _connectionString;

    public UserDataRepository(string connectionString) {
        _connectionString = connectionString;
    }


    public UserData GetUserData(string userid) {
        throw new NotImplementedException();
    }

    public UserData GetUserData(int userNo) {
        throw new NotImplementedException();
    }
}
