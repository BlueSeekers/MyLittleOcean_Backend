using Swashbuckle.AspNetCore.Annotations;

public class RankInfoDto {
    [SwaggerSchema("유저 랭킹")]
    public RankDetail? UserRank { get; set; }

    [SwaggerSchema("랭킹 목록")]
    public List<RankDetail>? RankList { get; set; }
}
