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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserFullDataByID([FromQuery] string userId) {
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserFullDataByNo([FromQuery] int userNo) {
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

    [HttpPost("update/name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateName([FromBody] UserNameUpdateDto request) {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 8) {
            return BadRequest(new { message = "닉네임이 잘못되었습니다. 확인해주세요." });
        }

        try {
            var result = await _userInfoService.UpdateUserName(request);
            if (!result.Success) {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = "User name updated successfully." });
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }
}
