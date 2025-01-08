public interface IRankingRepository
{
    int GetUserRanking(int userNo); //todo: 리턴값 유저정보로 변경
    List<Rank> GetTopRanks(int topN);
    void UpdateRank(Rank rank);
}
