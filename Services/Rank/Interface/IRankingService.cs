public interface IRankingService
{
    private const int DEFAULT_RANK_COUNT = 100; // 기본 랭킹 조회 개수
    Rank GetMyRanking(string gameType, int userNo);
    List<Rank> GetDailyRanks(string gameType, DateTime date, int count = DEFAULT_RANK_COUNT);
    List<Rank> GetMonthlyRanks(string gameType, DateTime date, int count = DEFAULT_RANK_COUNT);
    void UpdateRank(Rank rank);
}
