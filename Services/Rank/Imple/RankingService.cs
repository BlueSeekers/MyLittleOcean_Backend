public class RankingService : IRankingService {
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository) {
        _rankingRepository = rankingRepository;
    }

    // 내 랭킹 조회
    public async Task<RankDetail?> GetMyRanking(RankParamsDto rankParams) {
        return await _rankingRepository.GetUserRanking(rankParams);
    }

    // 전체 랭킹조회
    public async Task<List<RankDetail>> GetRankingList(RankParamsDto rankParams) {
        if (rankParams.dateType == DateType.Daily) {
            return await _rankingRepository.GetDailyRankList(rankParams);
        }
        else {
            return await _rankingRepository.GetMonthRankList(rankParams);
        }
    }

    // 랭킹 데이터 추가 또는 업데이트
    public async Task<ServiceResult<bool>> InsertRank(RankInsertDto rankParams) {
        var exist = await _rankingRepository.CheckRankExists(rankParams);
        if (exist) {
            var update = await _rankingRepository.UpdateRank(rankParams);
            if (update) {
                return new ServiceResult<bool>(true, "Success", true);
            }
            else {
                return new ServiceResult<bool>(false, "Failed Update Rank Data");
            }
        }
        else {
            var insert = await _rankingRepository.InsertRank(rankParams);
            if (insert) {
                return new ServiceResult<bool>(true, "Success", true);
            }
            else {
                return new ServiceResult<bool>(true, "Failed Insert Rank Data");
            }
        }
    }

    public string ToDateTimeString(DateTime date) {
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

