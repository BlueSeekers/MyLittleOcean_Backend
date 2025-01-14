
using Swashbuckle.AspNetCore.Annotations;

public class UserInfo {
    private int userNo;
    private string userId;
    private string userName;
    private string? userEmail;
    private bool accountLocked;
    private string? lockDate;
    private string? lastPwdDate;
    private string provider;
    private string createDate;
    private string updateDate;


    [SwaggerSchema("유저 식별번호")]
    public int UserNo { get => userNo; set => userNo = value; }

    [SwaggerSchema("유저 ID")]
    public string UserId { get => userId; set => userId = value; }

    [SwaggerSchema("유저 이름")]
    public string UserName { get => userName; set => userName = value; }

    [SwaggerSchema("유저 이메일")]
    public string? UserEmail { get => userEmail; set => userEmail = value; }

    [SwaggerSchema("정지 여부")]
    public bool AccountLocked { get => accountLocked; set => accountLocked = value; }

    [SwaggerSchema("정지 일자")]
    public string? LockDate { get => lockDate; set => lockDate = value; }

    [SwaggerSchema("마지막 패스워드 수정 일자")]
    public string? LastPwdDate { get => lastPwdDate; set => lastPwdDate = value; }

    [SwaggerSchema("제공자")]
    public string Provider { get => provider; set => provider = value; }     

    [SwaggerSchema("생성일자")]
    public string CreateDate { get => createDate; set => createDate = value; }

    [SwaggerSchema("수정일자")]
    public string UpdateDate { get => updateDate; set => updateDate = value; }
}
