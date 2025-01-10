
using Swashbuckle.AspNetCore.Annotations;

public class UserFullData {
    private UserInfo userInfo;

    private UserData userData;


    [SwaggerSchema("유저 상세 정보")]
    public UserInfo UserInfo { get => userInfo; set => userInfo = value; }

    [SwaggerSchema("유저 인게임 데이터")]
    public UserData UserData { get => userData; set => userData = value; }
}
