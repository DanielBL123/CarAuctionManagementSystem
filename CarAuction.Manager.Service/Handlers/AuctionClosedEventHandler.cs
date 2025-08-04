using CarAuction.Common.Global.Enum;
using CarAuction.Manager.Service.Events;
using CarAuction.RealTime.Interface;
using MediatR;

namespace CarAuction.Manager.Service.Handlers;

public class AuctionClosedEventHandler(IAuctionRepository auctionRepository, IAuctionNotifier auctionNotifier) : INotificationHandler<AuctionClosedEvent>
{
    public async Task Handle(AuctionClosedEvent notification, CancellationToken cancellationToken)
    {
        var auction = await auctionRepository.GetFirstAsync(notification.AuctionId);
        if (auction == null) return;

        auction.Status = AuctionStatus.Closed;
        auction.EndDate = DateTime.UtcNow;

        auctionRepository.Update(auction);
        await auctionRepository.SaveChangesAsync();

        await auctionNotifier.NotifyAuctionEndedAsync(notification.AuctionId);
    }
}
