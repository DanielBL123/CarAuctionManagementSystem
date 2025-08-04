using CarAuction.Common.Global.Enum;
using CarAuction.Manager.Service.Events;
using CarAuction.RealTime.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Manager.Service.Handlers;

public class VehicleBiddingClosedEventHandler(IBidRepository bidRepository, IVehicleRepository vehicleRepository, IAuctionNotifier auctionNotifier, IMediator mediator) : INotificationHandler<VehicleBiddingClosedEvent>
{
    public async Task Handle(VehicleBiddingClosedEvent notification, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetFirstAsync(notification.VehicleId);

        var winningBid = await bidRepository.AsQueryable()
            .Where(b => b.VehicleId == vehicle!.Id)
            .OrderByDescending(b => b.Amount)
            .Include(b => b.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (winningBid != null)
        {
            vehicle!.User = winningBid.User;
            vehicle.VehicleAction = VehicleAction.Sold;
            vehicle.CurrentBidAmount = winningBid.Amount;
        }
        else
        {
            vehicle!.VehicleAction = VehicleAction.None;
        }

        vehicleRepository.Update(vehicle);
        await vehicleRepository.SaveChangesAsync();

        await auctionNotifier.NotifyVehicleClosedAsync(vehicle.Id);

        var vehicles = notification.Vehicles.OrderBy(v => v.Id).ToList();
        var currentIndex = vehicles.FindIndex(v => v.Id == vehicle.Id);

        if (currentIndex >= 0 && currentIndex < vehicles.Count - 1)
        {
            var nextVehicle = vehicles[currentIndex + 1];
            await mediator.Publish(new VehicleBiddingOpenedEvent(notification.AuctionId, nextVehicle.Id, notification.Vehicles), cancellationToken);
        }
        else
        {
            await mediator.Publish(new AuctionClosedEvent(notification.AuctionId), cancellationToken);
        }
    }
}
