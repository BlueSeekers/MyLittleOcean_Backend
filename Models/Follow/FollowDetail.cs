using Swashbuckle.AspNetCore.Annotations;

public class FollowDetail {

    [SwaggerSchema("유저 식별번호")]
    public int userNo {  get; set; }

    [SwaggerSchema("유저 이름")]
    public string? userName { get; set; }
}
