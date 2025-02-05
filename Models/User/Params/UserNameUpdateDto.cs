using Swashbuckle.AspNetCore.Annotations;

public class UserNameUpdateDto {
    private string userId;
    private string name;

    [SwaggerSchema("���� ID")]
    public string UserId { get => userId; set => userId = value; }

    [SwaggerSchema("���� �г���")]
    public string Name { get => name; set => name = value; }
}