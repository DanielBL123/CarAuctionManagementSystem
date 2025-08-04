using MediatR;

namespace CarAuction.Manager.Service.Events;

public record VehicleBiddingOpenedEvent(int AuctionId, int VehicleId, List<VehicleDto> Vehicles) : INotification;
