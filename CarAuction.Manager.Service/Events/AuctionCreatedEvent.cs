using MediatR;

namespace CarAuction.Manager.Service.Events;

public record AuctionCreatedEvent(int AuctionId, List<VehicleDto> Vehicles) : INotification;
