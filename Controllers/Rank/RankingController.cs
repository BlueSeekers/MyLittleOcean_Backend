using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RankingController : ControllerBase {
    private readonly IRankingService _rankingService;

    public RankingController(IRankingService rankingService) {
        _rankingService = rankingService;
    }

    // 랭킹 데이터 조회 - 일간/ 월간
    [HttpPost("rank")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RankInfoDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RankInfoDto>> GetRanks([FromBody] RankParamsDto rankParams) {
        if (string.IsNullOrEmpty(rankParams.userId)) {
            return BadRequest(new { error = "Username Or GameType required" });
        }
        try {
            var userRankTask = _rankingService.GetMyRanking(rankParams);
            var rankListTask = _rankingService.GetRankingList(rankParams);

            await Task.WhenAll(userRankTask, rankListTask);

            var userRank = await userRankTask ?? new RankDetail();
            var rankList = await rankListTask ?? new List<RankDetail>();

            var ranks = new RankInfoDto {
                UserRank = userRank,
                RankList = rankList
            };

            return Ok(ranks);
        }
        catch (Exception ex) {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // 랭킹 데이터 추가 또는 업데이트
    [HttpPut("rank/insert")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> InsertRank([FromBody] RankInsertDto rankParams) {
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