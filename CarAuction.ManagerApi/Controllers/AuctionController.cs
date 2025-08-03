
[ApiController]
[Route("api/[controller]")]
public class AuctionsController(IAuctionService auctionService, IMapper mapper) : ControllerBase
{

    [HttpPost("create")]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionRequest request)
    {
        var auction = await auctionService.CreateAuctionAsync(request);
        return Ok(auction);
    }

    [HttpPost("close")]
    public async Task<IActionResult> CloseAuction([FromBody] CloseAuctionRequest request)
    {
        var result = await auctionService.CloseAuction(request);
        return Ok(result);
    }

    [HttpGet("auctions")]
    public async Task<IActionResult> GetActiveAuctions()
    {
        var result = await auctionService.GetActiveAuctions();
        return Ok(result);
        
    }

}