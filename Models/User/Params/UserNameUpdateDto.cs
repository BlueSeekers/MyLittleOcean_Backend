using Swashbuckle.AspNetCore.Annotations;

public class UserNameUpdateDto {
    private string userId;
    private string name;

    [SwaggerSchema("유저 ID")]
    public string UserId { get => userId; set => userId = value; }

    [SwaggerSchema("유저 닉네임")]
    public string Name { get => name; set => name = value; }
}