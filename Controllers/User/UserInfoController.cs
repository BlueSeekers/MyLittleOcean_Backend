using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserInfoController : ControllerBase {
    private IUserInfoService _userInfoService;
    public UserInfoController(IUserInfoService userInfoService) {
        _userInfoService = userInfoService;
    }

    [HttpGet("get/id")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserFullData))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserFullData>> GetUserFullDataByID([FromQuery] string userId) {
        if (userId == null) {
            return BadRequest("BadRequest Data");
        }
        try {
            var result = await _userInfoService.GetUserFullDataById(userId);
            if (!result.Success)
                return NotFound(new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }


    [HttpGet("get/no")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserFullData))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserFullData>> GetUserFullDataByNo([FromQuery] int userNo) {
        try {
            var result = await _userInfoService.GetUserFullDataByNo(userNo);
            if (!result.Success)
                return NotFound(new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [HttpPut("update/username")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUsernameById([FromBody] UpdateUsernameByIdDto request) {
        if (string.IsNullOrEmpty(request.NewUsername)) {
            return BadRequest(new { error = "Username is required" });
        }

        try {
            await _userInfoService.UpdateUserNameAsync(request.UserId, request.NewUsername);
            return Ok(new { message = "Username updated successfully" });
        }
        catch (ArgumentException ex) {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex) {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
