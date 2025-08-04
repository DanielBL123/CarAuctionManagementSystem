using MediatR;

namespace CarAuction.Manager.Service.Events;

public record AuctionClosedEvent(int AuctionId) : INotification;
