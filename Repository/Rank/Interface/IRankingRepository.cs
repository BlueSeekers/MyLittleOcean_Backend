public interface IRankingRepository
{
    RankDetail? GetUserRanking(string gameType, string startDate, string endDate, int userNo);
    List<RankDetail> GetTopRanksByPeriod(string gameType, string startDate, string endDate, int topN);
    Task<bool> InsertRank(RankInsertDto rank);
    Task<bool> UpdateRank(RankInsertDto rank);

    Task<bool> CheckRankExists(RankInsertDto rank);
}
