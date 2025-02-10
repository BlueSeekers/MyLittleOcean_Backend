public class UserDataService : IUserDataService {
    private readonly IUserDataRepository _userDataRepository;
    public UserDataService(IUserDataRepository userDataRepository) {
        _userDataRepository = userDataRepository;
    }

    /// <summary>
    /// UserNo로 코인 사용
    /// </summary>
    /// <param name="useDataDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseCoinByNo(UserUseDto useDataDto) {
        int useCoin = await _userDataRepository.UseCoinByNo(useDataDto);
        if (useCoin <= 0) {
            return new ServiceResult<int>(false, "코인을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useCoin);
    }

    /// <summary>
    /// User ID로 코인 사용
    /// </summary>
    /// <param name="useDataDto">useId : 유저 ID, amount : 수정 개수</param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseCoinByID(UserUseDto useDataDto) {
        int useCoin = await _userDataRepository.UseCoinByID(useDataDto);
        if (useCoin <= 0) {
            return new ServiceResult<int>(false, "코인을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useCoin);
    }

    /// <summary>
    /// User No로 토큰 사용
    /// </summary>
    /// <param name="useDataDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseTokenByNo(UserUseDto useDataDto) {
        int useToken = await _userDataRepository.UseTokenByNo(useDataDto);
        if (useToken <= 0) {
            return new ServiceResult<int>(false, "토큰을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useToken);
    }

    /// <summary>
    /// UserID로 토큰 사용
    /// </summary>
    /// <param name="useDataDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<int>> UseTokenByID(UserUseDto useDataDto) {
        int useToken = await _userDataRepository.UseTokenByID(useDataDto);
        if (useToken <= 0) {
            return new ServiceResult<int>(false, "토큰을 사용할 수 없습니다. 확인해주세요.", 0);
        }
        return new ServiceResult<int>(true, "Success", useToken);
    }

    /// <summary>
    /// 유저 게임 데이터 업데이트
    /// </summary>
    /// <param name="updateData">coinAmount, tokenAmount</param>
    public async Task<ServiceResult<UserData>> UserDataUpdate(UserUpdateDataDto updateData) {
        bool update = await _userDataRepository.UserDataUpdate(updateData);
        if (update) {
            UserData? userData = await _userDataRepository.GetUserDataById(updateData.userId);
            if (userData == null)
                throw new Exception("Unable to retrieve user data.");
            return new ServiceResult<UserData>(true, "Success", userData);
        }
        else {
            return new ServiceResult<UserData>(false, "Failed Update User Data");
        }
    }

    public async Task<ServiceResult<UserData>> RewardPayment(RewardParamsDto rewardParams) {
        bool update = await _userDataRepository.RewardPayment(rewardParams);
        if (update) {
            UserData? userData = await _userDataRepository.GetUserDataById(rewardParams.userId);
            if (userData == null)
                throw new Exception("Unable to retrieve user data.");
            return new ServiceResult<UserData>(true, "Success", userData);
        } else {
            return new ServiceResult<UserData>(false, "Failed Payment Reward");
        }
    }
}
