﻿using Swashbuckle.AspNetCore.Annotations;

public class AuthCreateDto {

    [SwaggerSchema("유저 ID")]
    public String userId { get; set; }

    [SwaggerSchema("유저 이름")]
    public String? userName { get; set; }

    [SwaggerSchema("유저 패스워드")]
    public String? userPwd { get; set; }

    [SwaggerSchema("유저 이메일")]
    public String? userEmail { get; set; }

    [SwaggerSchema("제공자 (GPGS, NAVER, KAKAO)")]
    public String provider { get; set; }

}

