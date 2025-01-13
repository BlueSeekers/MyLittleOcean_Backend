using Swashbuckle.AspNetCore.Annotations;

public class RankingInfoDto
{
    [SwaggerSchema("유저 랭킹")]
    public Rank? UserRank { get; set; }

    [SwaggerSchema("일간 랭킹")]
    public List<Rank>? TodayTopRanks { get; set; }

    [SwaggerSchema("월간 랭킹")]
    public List<Rank>? MonthTopRanks { get; set; }
}
