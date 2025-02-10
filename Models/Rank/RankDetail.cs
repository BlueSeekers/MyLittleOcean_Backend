using Swashbuckle.AspNetCore.Annotations;

public class RankDetail
{
    private int rankNo;
    private int userNo;
    private int rankValue;
    private string? userName;
    private string? createDate;
    private string? gameType;

    [SwaggerSchema("랭킹 식별번호")]
    public int RankNo { get => rankNo; set => rankNo = value; }
    [SwaggerSchema("유저 No")]
    public int UserNo { get => userNo; set => userNo = value; }
    [SwaggerSchema("점수")]
    public int RankValue { get => rankValue; set => rankValue = value; }
    [SwaggerSchema("유저 이름")]
    public string UserName { get => userName; set => userName = value; }
    [SwaggerSchema("등록일자")]
    public string CreateDate { get => createDate; set => createDate = value; }
    [SwaggerSchema("게임 종류")]
    public string GameType { get => gameType; set => gameType = value; }
}