using Swashbuckle.AspNetCore.Annotations;

public class RankInsertDto {

    [SwaggerSchema("유저 식별번호")]
    public int userNo {  get; set; }

    [SwaggerSchema("점수")]
    public int rankValue { get; set; }

    [SwaggerSchema("게임 종류")]
    public string gameType { get; set; }
}