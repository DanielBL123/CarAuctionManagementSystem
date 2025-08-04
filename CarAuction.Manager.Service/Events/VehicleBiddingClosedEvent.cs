using MediatR;

namespace CarAuction.Manager.Service.Events;

public record VehicleBiddingClosedEvent(int AuctionId, int VehicleId, List<VehicleDto> Vehicles) : INotification;
