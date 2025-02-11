using Swashbuckle.AspNetCore.Annotations;

public enum GameType{
    Coral = 0,
    Abyss,
    Dive,
    Seeker

}

public enum DateType {
    Daily = 0,
    Month
}

public class RankParamsDto {

    [SwaggerSchema("유저 ID")]
    public string userId {  get; set; }

    [SwaggerSchema("게임 Type (Coral, Abyss, Dice, Seeker)")]
    public GameType gameType {  get; set; }

    [SwaggerSchema("날짜 Type (Daily, Month)")]
    public DateType dateType { get; set; }

    [SwaggerSchema("조회 개수 (Default : 50)")]
    public int rankCount { get; set; } = 50;
}
