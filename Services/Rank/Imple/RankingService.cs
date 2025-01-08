
public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;
    private const int DEFAULT_RANK_COUNT = 100; // 기본 랭킹 조회 개수

    public RankingService(IRankingRepository rankingRepository)
    {
        _rankingRepository = rankingRepository;
    }

    // 랭킹 데이터 조회
    public List<Rank> GetRanks()
    {
        return _rankingRepository.GetTopRanks(DEFAULT_RANK_COUNT);
    }
    // 특정 개수의 랭킹 데이터 조회
    public List<Rank> GetTopRanks(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentException("Count must be greater than 0", nameof(count));
        }

        return _rankingRepository.GetTopRanks(count);
    }
    // 특정 유저의 랭킹 순위 조회
    public int GetUserRanking(int userNo)
    {
        if (userNo <= 0)
        {
            throw new ArgumentException("UserNo must be greater than 0", nameof(userNo));
        }

        return _rankingRepository.GetUserRanking(userNo);
    }

    // 랭킹 데이터 추가 또는 업데이트
    public void UpdateRank(Rank rank)
    {
        if (rank == null)
        {
            throw new ArgumentNullException(nameof(rank));
        }

        if (rank.UserNo <= 0)
        {
            throw new ArgumentException("UserNo must be greater than 0", nameof(rank));
        }

        if (rank.RankValue < 0)
        {
            throw new ArgumentException("RankValue cannot be negative", nameof(rank));
        }

        _rankingRepository.UpdateRank(rank);
    }
    // 랭킹 정보 조회 (유저의 순위와 상위 랭킹 리스트)
    public RankingInfoDto GetRankingInfo(int userNo, int topCount = DEFAULT_RANK_COUNT)
    {
        if (userNo <= 0)
        {
            throw new ArgumentException("UserNo must be greater than 0", nameof(userNo));
        }

        if (topCount <= 0)
        {
            throw new ArgumentException("TopCount must be greater than 0", nameof(topCount));
        }

        var userRank = GetUserRanking(userNo);
        var topRanks = GetTopRanks(topCount);

        return new RankingInfoDto
        {
            UserRank = userRank,
            TopRanks = topRanks
        };
    }
}

