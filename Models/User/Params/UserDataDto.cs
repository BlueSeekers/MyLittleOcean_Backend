using Swashbuckle.AspNetCore.Annotations;

public class UserDataDto {
    private int? userNo;
    private string? userId;
    private int amount;


    [SwaggerSchema("유저 고유번호")]
    public int? UserNo { get => userNo; set => userNo = value; }

    [SwaggerSchema("유저 ID")]
    public string? UserId { get => userId; set => userId = value; }

    [SwaggerSchema("수정할 개수")]
    public int Amount { get => amount; set => amount = value; }
}
