
[ApiController]
[Route("api/[controller]")]
public class AuctionsController(IAuctionService auctionService) : ControllerBase
{

    [HttpPost("create")]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionRequest request)
    {
        try
        {
            var auction = await auctionService.CreateAuctionAsync(request);
            return Ok(auction);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.Message);
        }
        
    }

    [HttpPost("close")]
    public async Task<IActionResult> CloseAuction([FromBody] CloseAuctionRequest request)
    {
        try
        {
            var result = await auctionService.CloseAuction(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet("auctions")]
    public async Task<IActionResult> GetActiveAuctions()
    {
        try
        {
            var result = await auctionService.GetActiveAuctions();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

}