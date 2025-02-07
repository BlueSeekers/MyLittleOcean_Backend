using Swashbuckle.AspNetCore.Annotations;

public class AuthCreateDto {

    [SwaggerSchema("유저 ID")]
    public String userId { get; set; }

    [SwaggerSchema("유저 이름")]
    public String? userName { get; set; }

    [SwaggerSchema("유저 이메일")]
    public String? userEmail { get; set; }

    [SwaggerSchema("유저 이미지")]
    public String? userImg { get; set; }

    [SwaggerSchema("제공자")]
    public String provider { get; set; }
}

