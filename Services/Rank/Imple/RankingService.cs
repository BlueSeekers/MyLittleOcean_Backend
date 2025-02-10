public class RankingService : IRankingService {
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository) {
        _rankingRepository = rankingRepository;
    }

    // 내 랭킹 조회
    public RankDetail GetMyRanking(string gameType, int userNo) {
        ValidateGameType(gameType);
        ValidateUserNo(userNo);

        return _rankingRepository.GetUserRanking(gameType, userNo);
    }
    // 전체 랭킹(일간) 조회
    public List<RankDetail> GetDailyRanks(string gameType, DateTime date, int count) {
        ValidateGameType(gameType);

        var startDate = date.Date; // 해당일 00:00:00
        var endDate = startDate.AddDays(1).AddSeconds(-1); // 해당일 23:59:59

        return _rankingRepository.GetTopRanksByPeriod(gameType, ToDateTimeString(startDate), ToDateTimeString(endDate), count);
    }
    // 월별 랭킹 조회
    public List<RankDetail> GetMonthlyRanks(string gameType, DateTime date, int count) {
        ValidateGameType(gameType);

        var startDate = new DateTime(date.Year, date.Month, 1);
        var endDate = startDate.AddMonths(1).AddSeconds(-1);

        return _rankingRepository.GetTopRanksByPeriod(gameType, ToDateTimeString(startDate), ToDateTimeString(endDate), count);
    }

    // 랭킹 데이터 추가 또는 업데이트
    public async Task<ServiceResult<bool>> InsertRank(RankInsertDto rankParams) {
        ValidateRank(rankParams);
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

    private void ValidateGameType(string gameType) {
        if (string.IsNullOrEmpty(gameType)) {
            throw new ArgumentException("GameType cannot be empty", nameof(gameType));
        }
    }

    private void ValidateUserNo(long userNo) {
        if (userNo <= 0) {
            throw new ArgumentException("UserNo must be greater than 0", nameof(userNo));
        }
    }

    private void ValidateRank(RankInsertDto rank) {
        if (rank == null) {
            throw new ArgumentNullException(nameof(rank));
        }

        ValidateGameType(rank.gameType);
        ValidateUserNo(rank.userNo);

        if (rank.rankValue < 0) {
            throw new ArgumentException("RankValue cannot be negative", nameof(rank));
        }
    }
}

