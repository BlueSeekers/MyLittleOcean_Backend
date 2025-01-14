using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserDataController : ControllerBase {
    private IUserDataService _userDataService;

    public UserDataController(IUserDataService userDataService) {
        _userDataService = userDataService;
    }

    [HttpPost("use/coin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult UseCoin([FromBody] UseDataDto useDataDto) {
        if (useDataDto.UserNo == null && useDataDto.UserId == null) {
            return BadRequest("No information exists");
        }
        try {
            if (useDataDto.UserNo != null) {
                int use = _userDataService.UseTokenByID(useDataDto);
                return (use > 0) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else if (useDataDto.UserId != null) {
                int use = _userDataService.UseTokenByNo(useDataDto);
                return (use > 0) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else {
                return StatusCode(500, new { message = "Failed to Use Coin" });
            }
        }
        catch (Exception e) {
            return StatusCode(500, new { message = "Failed to Use Coin" });
        }
    }

    [HttpPost("use/token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult UseToken([FromBody] UseDataDto useDataDto) {
        if (useDataDto.UserNo == null && useDataDto.UserId == null) {
            return BadRequest("No information exists");
        }
        try {
            if (useDataDto.UserNo != null) {
                int use = _userDataService.UseTokenByID(useDataDto);
                return (use > 0) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else if (useDataDto.UserId != null) {
                int use = _userDataService.UseTokenByNo(useDataDto);
                return (use > 0) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else {
                return StatusCode(500, new { message = "Failed to Use Coin" });
            }
        }
        catch (Exception e) {
            return StatusCode(500, new { message = "Failed to Use Coin" });
        }
    }
}
