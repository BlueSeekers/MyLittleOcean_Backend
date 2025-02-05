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
    public async Task<ServiceResult<int>> UseCoinByNo(UserDataDto useDataDto) {
        int useCoin = await _userDataRepository.UseCoinByNo(useDataDto);
        if (useCoin <= 0) {
            return new ServiceResult<int>(false, "코인을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useCoin);
    }

    /// <summary>
    /// User ID로 코인 사용
    /// </summary>
    /// <param name="useByIdDto">useId : 유저 ID, amount : 수정 개수</param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseCoinByID(UserDataDto useDataDto) {
        int useCoin = await _userDataRepository.UseCoinByID(useDataDto);
        if (useCoin <= 0) {
            return new ServiceResult<int>(false, "코인을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useCoin);
    }   

    /// <summary>
    /// User No로 토큰 사용
    /// </summary>
    /// <param name="useByNoDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseTokenByNo(UserDataDto useDataDto) {
        int useToken = await _userDataRepository.UseTokenByNo(useDataDto);
        if (useToken <= 0) {
            return new ServiceResult<int>(false, "토큰을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useToken);
    }

    /// <summary>
    /// UserID로 토큰 사용
    /// </summary>
    /// <param name="useByIdDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseTokenByID(UserDataDto useDataDto) {
        int useToken = await _userDataRepository.UseTokenByID(useDataDto);
        if (useToken <= 0) {
            return new ServiceResult<int>(false, "토큰을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useToken);
    }
}
