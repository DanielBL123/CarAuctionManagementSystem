using CarAuction.Dto;
using CarAuction.RealTime;
using Microsoft.AspNetCore.SignalR;

namespace CarAuction.ManagerApi.Controllers;

[ApiController]
[Route("notify")]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<AuctionHub> _hubContext;
    public NotificationController(IHubContext<AuctionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("BidPlaced")]
    public async Task<IActionResult> NotifyAuctionCreated([FromBody] BidDto bid)
    {
        await _hubContext.Clients.All.SendAsync("BidPlaced", bid);
        return Ok();
    }
}
