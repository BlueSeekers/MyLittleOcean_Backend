
using System.Security.AccessControl;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository)
    {
        _rankingRepository = rankingRepository;
    }

    // 내 랭킹 조회
    public Rank GetMyRanking(string gameType, int userNo)
    {
        ValidateGameType(gameType);
        ValidateUserNo(userNo);

        return _rankingRepository.GetUserRanking(gameType, userNo);
    }
    // 전체 랭킹(일간) 조회
    public List<Rank> GetDailyRanks(string gameType, DateTime date, int count)
    {
        ValidateGameType(gameType);

        var startDate = date.Date; // 해당일 00:00:00
        var endDate = startDate.AddDays(1).AddSeconds(-1); // 해당일 23:59:59

        return _rankingRepository.GetTopRanksByPeriod(gameType, startDate.ToString(), endDate.ToString(), count);
    }
    // 월별 랭킹 조회
    public List<Rank> GetMonthlyRanks(string gameType, DateTime date, int count)
    {
        ValidateGameType(gameType);

        var startDate = new DateTime(date.Year, date.Month, 1);
        var endDate = startDate.AddMonths(1).AddSeconds(-1);

        return _rankingRepository.GetTopRanksByPeriod(gameType, startDate.ToString(), endDate.ToString(), count);
    }
    // 랭킹 데이터 추가 또는 업데이트
    public void UpdateRank(Rank rank)
    {
        ValidateRank(rank);
        _rankingRepository.UpdateRank(rank);
    }

    private void ValidateGameType(string gameType)
    {
        if (string.IsNullOrEmpty(gameType))
        {
            throw new ArgumentException("GameType cannot be empty", nameof(gameType));
        }
    }

    private void ValidateUserNo(long userNo)
    {
        if (userNo <= 0)
        {
            throw new ArgumentException("UserNo must be greater than 0", nameof(userNo));
        }
    }

    private void ValidateRank(Rank rank)
    {
        if (rank == null)
        {
            throw new ArgumentNullException(nameof(rank));
        }

        ValidateGameType(rank.GameType);
        ValidateUserNo(rank.UserNo);

        if (rank.RankValue < 0)
        {
            throw new ArgumentException("RankValue cannot be negative", nameof(rank));
        }
    }
}

