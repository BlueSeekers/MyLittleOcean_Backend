public class UserDataService : IUserDataService {
    private readonly IUserDataRepository _userDataRepository;
    public UserDataService(IUserDataRepository userDataRepository) {
        _userDataRepository = userDataRepository;
    }

    /// <summary>
    /// UserNo로 코인 사용
    /// </summary>
    /// <param name="useByNoDto"></param>
    /// <returns></returns>
    public int UseTokenByNo(UseDataDto useDataDto) {
        return _userDataRepository.UseCoinByNo(useDataDto);
    }

    /// <summary>
    /// User ID로 코인 사용
    /// </summary>
    /// <param name="useByIdDto">useId : 유저 ID, amount : 수정 개수</param>
    /// <returns></returns>
    public int UseCoinByID(UseDataDto useDataDto) {
        return _userDataRepository.UseCoinByID(useDataDto);
    }   

    /// <summary>
    /// User No로 토큰 사용
    /// </summary>
    /// <param name="useByNoDto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int UsertTokenByNo(UseDataDto useDataDto) {
        return _userDataRepository.UseTokenByNo(useDataDto);
    }

    /// <summary>
    /// UserID로 토큰 사용
    /// </summary>
    /// <param name="useByIdDto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int UseTokenByID(UseDataDto useDataDto) {
        return _userDataRepository.UseTokenByID(useDataDto);
    }
}
