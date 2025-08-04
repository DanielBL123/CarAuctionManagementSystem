using CarAuction.Manager.Service.Events;
using MediatR;

namespace CarAuction.Manager.Service.Handlers;

public class AuctionCreatedEventHandler(IMediator mediator) : INotificationHandler<AuctionCreatedEvent>
{
    public async Task Handle(AuctionCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Vehicles.Count == 0) return;

        var firstVehicle = notification.Vehicles.First();

        await mediator.Publish(new VehicleBiddingOpenedEvent(notification.AuctionId, firstVehicle.Id, notification.Vehicles), cancellationToken);
    }
}
