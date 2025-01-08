public class AuthService : IAuthService {
    private readonly IAuthRepository _userRepository;

    public AuthService(IAuthRepository userRepository) {
        _userRepository = userRepository;
    }

    public int CreateUser(AuthCreateDto userCreateDto) {
        int userNo = _userRepository.CreateUser(userCreateDto);
        Console.WriteLine($"CreateUser returned: {userNo}");

        if (userNo > 0) {
            return _userRepository.CreateDefaultUserData(userNo);
        }
        else return 0;
    }
}
