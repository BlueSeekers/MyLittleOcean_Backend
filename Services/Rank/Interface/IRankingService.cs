public interface IRankingService
{
    private const int DEFAULT_RANK_COUNT = 100; // 기본 랭킹 조회 개수
    RankDetail GetMyRanking(string gameType, int userNo);
    List<RankDetail> GetDailyRanks(string gameType, DateTime date, int count = DEFAULT_RANK_COUNT);
    List<RankDetail> GetMonthlyRanks(string gameType, DateTime date, int count = DEFAULT_RANK_COUNT);
    Task<ServiceResult<bool>> InsertRank(RankInsertDto rankParams);
}
