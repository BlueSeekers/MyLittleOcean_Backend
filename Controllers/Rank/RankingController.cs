using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RankingController : ControllerBase
{
    private readonly RankingService _rankingService;

    public RankingController(RankingService rankingService)
    {
        _rankingService = rankingService;
    }

    // 랭킹 데이터 조회
    [HttpGet]
    public ActionResult<IEnumerable<Rank>> GetRanks()
    {
        var ranks = _rankingService.GetRanks();
        return Ok(ranks);
    }

    // 랭킹 데이터 추가 또는 업데이트
    [HttpPost]
    public IActionResult UpdateRank([FromBody] Rank rank)
    {
        _rankingService.UpdateRank(rank);
        return Ok(rank);
    }
}