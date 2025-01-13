using Swashbuckle.AspNetCore.Annotations;

public class RankingInfoDto
{
    [SwaggerSchema("유저 랭킹")]
    public RankDetail? UserRank { get; set; }

    [SwaggerSchema("일간 랭킹")]
    public List<RankDetail>? TodayTopRanks { get; set; }

    [SwaggerSchema("월간 랭킹")]
    public List<RankDetail>? MonthTopRanks { get; set; }
}
