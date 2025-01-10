
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLittleOcean.Models.Follow;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "Follow")]
[Authorize]
public class FollowController : ControllerBase {
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService) {
        _followService = followService;
    }

    /// <summary>
    /// 팔로우 하기 
    /// </summary>
    /// <param name="request">targetUserNo : 대상 유저 No, followUserNo : 팔로우 유저 No</param>
    /// <returns>ActionResult 200</returns>
    [HttpPost("submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateFollow([FromBody] FollowCreateRequestDto request) {
        if (request.targetUserNo == 0 || request.followUserNo == 0) {
            return BadRequest("BadRequest Data");
        }
        try {
            int response = _followService.CreateFollow(request);
            if (response > 0)
                return Ok(new { message = "Following Completed Successfully" });
            else
                return StatusCode(500, new { message = "Failed to following" });
        }
        catch (Exception e) {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// 팔로우 취소
    /// </summary>
    /// <param name="request">targetUserNo : 대상 유저 No, followUserNo : 팔로우 유저 No</param>
    /// <returns>ActionResult 200</returns>
    [HttpPost("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult DeleteFollow([FromBody] FollowCreateRequestDto request) {
        if (request.targetUserNo == 0 || request.followUserNo == 0) {
            return BadRequest("BadRequest Data");
        }
        try {
            int response = _followService.DeleteFollow(request);
            if (response > 0)
                return Ok(new { message = "UnFollowing Completed Successfully" });
            else
                return StatusCode(500, new { message = "Failed to unfollowing" });
        }
        catch (Exception e) {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// 특정 유저가 팔로우한 목록
    /// </summary>
    /// <param name="userNo">유저 식별 번호</param>
    /// <returns> 유저 목록 </returns>
    [HttpGet("following")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetFollowingList([FromQuery] int userNo) {
        if (userNo <= 0) {
            return BadRequest("BadRequest Data");
        }
        try {
            List<FollowDetail> followers = _followService.GetFollowingList(userNo);
            if (followers.Count > 0)
                return Ok(followers);
            else
                return Ok(new List<FollowDetail>());
        }
        catch (Exception e) {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// 특정 유저를 팔로우한 목록
    /// </summary>
    /// <param name="userNo">유저 식별 번호</param>
    /// <returns> 유저 목록 </returns>
    [HttpGet("follower")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetFollowersList([FromQuery]int userNo) {
        if(userNo <= 0) {
            return BadRequest("BadRequest Data");
        }
        try {
            List<FollowDetail> followers = _followService.GetFollowersList(userNo);
            if (followers.Count > 0)
                return Ok(followers);
            else
                return Ok(new List<FollowDetail>());
        }
        catch (Exception e) {
            return BadRequest(new { error = e.Message });
        }
    }
}
