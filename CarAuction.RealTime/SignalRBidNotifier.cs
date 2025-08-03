using CarAuction.Dto;
using CarAuction.RealTime.Interface;
using Microsoft.AspNetCore.SignalR;

namespace CarAuction.RealTime;

public class SignalRBidNotifier(IHubContext<AuctionHub> hubContext) : IBidNotifier
{
    public async Task NotifyBidPlacedAsync(BidDto bid)
    {
        await hubContext.Clients.All.SendAsync("BidPlaced", bid);
    }
}
