using Swashbuckle.AspNetCore.Annotations;

public class RankInsertDto {

    [SwaggerSchema("유저 ID")]
    public int userId {  get; set; }

    [SwaggerSchema("점수")]
    public int rankValue { get; set; }

    [SwaggerSchema("게임 종류")]
    public GameType gameType { get; set; }
}