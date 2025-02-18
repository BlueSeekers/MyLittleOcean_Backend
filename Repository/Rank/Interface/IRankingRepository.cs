﻿public interface IRankingRepository
{
    Task<RankDetail?> GetDailyUserRanking(RankParamsDto rankParams);
    Task<RankDetail?> GetMonthUserRanking(RankParamsDto rankParams);
    Task<List<RankDetail>> GetDailyRankList(RankParamsDto rankParams);
    Task<List<RankDetail>> GetMonthRankList(RankParamsDto rankParams);

    Task<bool> InsertRank(RankInsertDto rank);
    Task<bool> UpdateRank(RankInsertDto rank);

    Task<bool> CheckRankExists(RankInsertDto rank);
    Task<bool> CheckRankLow(RankInsertDto rank);
}
