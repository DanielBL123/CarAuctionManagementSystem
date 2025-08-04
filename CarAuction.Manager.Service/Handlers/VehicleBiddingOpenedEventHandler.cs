using CarAuction.Common.Global.Enum;
using CarAuction.Manager.Service.Events;
using CarAuction.RealTime.Interface;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuction.Manager.Service.Handlers;

public class VehicleBiddingOpenedEventHandler(
    IVehicleRepository vehicleRepository,
    IAuctionNotifier auctionNotifier,
    IMediator mediator, IServiceScopeFactory scopeFactory) : INotificationHandler<VehicleBiddingOpenedEvent>
{
    public async Task Handle(VehicleBiddingOpenedEvent notification, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetFirstAsync(notification.VehicleId);
        
        vehicle!.VehicleAction = VehicleAction.Liciting;
        vehicleRepository.Update(vehicle);
        await vehicleRepository.SaveChangesAsync();

        await auctionNotifier.NotifyVehicleOpenAsync(new VehicleDto
        {
            Manufacturer = vehicle.Manufacturer,
            Model = vehicle.Model,
            CurrentBidAmount = vehicle.CurrentBidAmount
        });


        _ = Task.Run(async () =>
        {
            using var scope = scopeFactory.CreateScope();
            var mediatorScoped = scope.ServiceProvider.GetRequiredService<IMediator>();

            await Task.Delay(TimeSpan.FromMinutes(1));
            await mediatorScoped.Publish(new VehicleBiddingClosedEvent(notification.AuctionId, notification.VehicleId, notification.Vehicles));
        }, cancellationToken);

    }
}