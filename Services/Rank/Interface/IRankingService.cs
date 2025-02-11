public interface IRankingService
{
    Task<RankDetail?> GetMyRanking(RankParamsDto rankParams);
    Task<List<RankDetail>> GetRankingList(RankParamsDto rankParams);
    Task<ServiceResult<bool>> InsertRank(RankInsertDto rankParams);
}
