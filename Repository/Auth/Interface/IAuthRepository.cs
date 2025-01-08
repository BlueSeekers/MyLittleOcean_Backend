public interface IAuthRepository {

    int CreateUser(AuthCreateDto userCreateDto);

    int CreateDefaultUserData(int userNo);
}
