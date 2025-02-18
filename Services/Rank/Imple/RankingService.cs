public enum RankInsertStatus{
    UPDATE_SUCCESS,
    UPDATE_FAILED,
    INSERT_SUCCESS,
    INSERT_FAILD,
    DUPLICATE_ENTRY
}

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
    public async Task<ServiceResult<RankInsertStatus>> InsertRank(RankInsertDto rankParams) {
        //오늘 날짜의 해당 게임 데이터가 있으면 true
        var exist = await _rankingRepository.CheckRankExists(rankParams);
        if (exist) {
            var dataLow = await _rankingRepository.CheckRankLow(rankParams);
            if (!dataLow) {
                return new ServiceResult<RankInsertStatus>(false, "The value that already exists is greater", RankInsertStatus.DUPLICATE_ENTRY);
            }
            var update = await _rankingRepository.UpdateRank(rankParams);
            if (update) {
                return new ServiceResult<RankInsertStatus>(true, "Success", RankInsertStatus.UPDATE_SUCCESS);
            }
            else {
                return new ServiceResult<RankInsertStatus>(false, "Failed Update Rank Data", RankInsertStatus.UPDATE_FAILED);
            }
        }
        else {
            var insert = await _rankingRepository.InsertRank(rankParams);
            if (insert) {
                return new ServiceResult<RankInsertStatus>(true, "Success", RankInsertStatus.INSERT_SUCCESS);
            }
            else {
                return new ServiceResult<RankInsertStatus>(true, "Failed Insert Rank Data", RankInsertStatus.INSERT_FAILD);
            }
        }
    }

    public string ToDateTimeString(DateTime date) {
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

