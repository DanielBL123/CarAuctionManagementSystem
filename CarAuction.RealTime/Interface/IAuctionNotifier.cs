using CarAuction.Dto;

namespace CarAuction.RealTime.Interface;

public interface IAuctionNotifier
{
    Task NotifyAuctionCreatedAsync(AuctionDto auction);
    Task NotifyVehicleOpenAsync(VehicleDto vehicle);
    Task NotifyVehicleClosedAsync(int vehicleId);
    Task NotifyAuctionEndedAsync(int auctionId);
}
