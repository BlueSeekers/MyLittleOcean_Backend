public interface IRankingRepository
{
    RankDetail? GetUserRanking(string gameType, int userNo);
    List<RankDetail> GetTopRanksByPeriod(string gameType, string startDate, string endDate, int topN);
    void UpdateRank(RankDetail rank);
}
