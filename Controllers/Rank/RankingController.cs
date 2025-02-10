using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RankingController : ControllerBase
{
    private readonly IRankingService _rankingService;

    public RankingController(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    // 랭킹 데이터 조회
    [HttpGet("rank")]
    public ActionResult<IEnumerable<RankDetail>> GetRanks(string gameType, int userNo)
    {
        RankingInfoDto rankingInfo = new RankingInfoDto();
        rankingInfo.UserRank = _rankingService.GetMyRanking(gameType, userNo);
        rankingInfo.TodayTopRanks = _rankingService.GetDailyRanks(gameType, DateTime.Today);
        rankingInfo.MonthTopRanks = _rankingService.GetMonthlyRanks(gameType, DateTime.Today);
        return Ok(rankingInfo);
    }

    // 랭킹 데이터 추가 또는 업데이트
    [HttpPost("rank")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> InsertRank([FromBody] RankInsertDto rankParams)
    {
        try {
            var result = await _rankingService.InsertRank(rankParams);
            if (!result.Success)
                return NotFound(new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception ex) {
            return StatusCode(500, new { error = ex.Message });
        }       
    }
}