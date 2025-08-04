using CarAuction.Dto;
using CarAuction.RealTime.Interface;
using Microsoft.AspNetCore.SignalR;

namespace CarAuction.RealTime;

public class SignalRAuctionNotifier : IAuctionNotifier
{
    private readonly IHubContext<AuctionHub> _hubContext;

    public SignalRAuctionNotifier(IHubContext<AuctionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyAuctionCreatedAsync(AuctionDto auction) =>
        await _hubContext.Clients.All.SendAsync("AuctionCreated", auction);

    public async Task NotifyVehicleOpenAsync(VehicleDto vehicle) =>
        await _hubContext.Clients.All.SendAsync("VehicleOpen", vehicle);

    public async Task NotifyVehicleClosedAsync(int vehicleId) =>
        await _hubContext.Clients.All.SendAsync("VehicleClosed", vehicleId);


    public async Task NotifyAuctionEndedAsync(int auctionId) =>
        await _hubContext.Clients.All.SendAsync("AuctionEnded", auctionId);

    public async Task NotifyBidPlacedAsync(BidDto bid)
    {
        await _hubContext.Clients.All.SendAsync("BidPlaced", bid);
    }
}
