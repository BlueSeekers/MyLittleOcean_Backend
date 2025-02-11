using Swashbuckle.AspNetCore.Annotations;

public class RankDetail {

    [SwaggerSchema("랭크 순위")]
    public int rank { get; set; }
    [SwaggerSchema("유저 No")]
    public int userNo { get; set; }
    [SwaggerSchema("점수")]
    public int rankValue { get; set; }
    [SwaggerSchema("유저 이름")]
    public string? userName { get; set; }
    [SwaggerSchema("등록일자")]
    public string? createDate { get; set; }
}