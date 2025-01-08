using Swashbuckle.AspNetCore.Annotations;

public class RankingInfoDto
{
    [SwaggerSchema("유저 랭킹")]
    public int UserRank { get; set; }

    [SwaggerSchema("탑 유저 랭킹 리스트")]
    public List<Rank>? TopRanks { get; set; }
}
