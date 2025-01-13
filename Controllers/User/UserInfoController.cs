using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "UserInfo")]
public class UserInfoController : ControllerBase {
    private IUserInfoService _userInfoService;
    public UserInfoController(IUserInfoService userInfoService) {
        _userInfoService = userInfoService;
    }

    [HttpGet("get/byID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetUserFullDataByID([FromQuery] string userId) {
        if (userId == null) {
            return BadRequest("BadRequest Data");
        }
        try {
            UserFullData userFullData = _userInfoService.GetUserFullDataById(userId);
            return userFullData == null ? NotFound() : Ok(userFullData);
        }
        catch (Exception e) {
            return StatusCode(500, new { message = "Failed to Geu User Data" });
        }
    }


    [HttpGet("get/byNo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetUserFullDataByNo([FromQuery] int userNo) {
        try {
            UserFullData userFullData = _userInfoService.GetUserFullDataByNo(userNo);
            return userFullData == null ? NotFound() : Ok(userFullData);
        }
        catch (Exception e) {
            return StatusCode(500, new { message = "Failed to Geu User Data" });
        }
    }
}
