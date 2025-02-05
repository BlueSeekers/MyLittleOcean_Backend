using Swashbuckle.AspNetCore.Annotations;

public class UpdateUsernameByIdDto {

    [SwaggerSchema("유저 ID")]
    public string UserId { get; set; }

    [SwaggerSchema("새 유저 닉네임")]
    public string NewUsername { get; set; }
}