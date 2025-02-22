﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserDataController : ControllerBase {
    private IUserDataService _userDataService;

    public UserDataController(IUserDataService userDataService) {
        _userDataService = userDataService;
    }

    [HttpPatch("use/coin")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UseCoin([FromBody] UserUseDto useDataDto) {
        if (useDataDto.UserNo == null && useDataDto.UserId == null) {
            return BadRequest("No information exists");
        }
        try {
            if (useDataDto.UserNo != null) {
                var use = await _userDataService.UseTokenByID(useDataDto);
                return (use.Success) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else if (useDataDto.UserId != null) {
                var use = await _userDataService.UseTokenByNo(useDataDto);
                return (use.Success) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else {
                return StatusCode(500, new { message = "Failed to Use Coin" });
            }
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [HttpPatch("use/token")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UseToken([FromBody] UserUseDto useDataDto) {
        if (useDataDto.UserNo == null && useDataDto.UserId == null) {
            return BadRequest("No information exists");
        }
        try {
            if (useDataDto.UserNo != null) {
                var use = await _userDataService.UseTokenByID(useDataDto);
                return (use.Success) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else if (useDataDto.UserId != null) {
                var use = await _userDataService.UseTokenByNo(useDataDto);
                return (use.Success) ? Ok("Coin Use Successfully")
                    : StatusCode(500, new { message = "Failed to Use Coin" });
            }
            else {
                return StatusCode(500, new { message = "Failed to Use Coin" });
            }
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [HttpPatch("update/data")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserData))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserData>> UpdateData([FromBody] UserUpdateDataDto useDataDto) {
        try {
            var data = await _userDataService.UserDataUpdate(useDataDto);
            return data.Success ? Ok(data.Data) : StatusCode(500, new { message = data.Message });
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [HttpPatch("get/reward")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserData))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserData>> RewardPayment([FromBody] RewardParamsDto rewardParams) {
        try {
            var data = await _userDataService.RewardPayment(rewardParams);
            return data.Success ? Ok(data.Data) : StatusCode(500, new { message = data.Message });
        }
        catch (Exception e) {
            return StatusCode(500, new { message = e.Message });
        }
    }
}