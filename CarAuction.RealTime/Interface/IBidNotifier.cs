using CarAuction.Dto;

namespace CarAuction.RealTime.Interface;

public interface IBidNotifier
{
    Task NotifyBidPlacedAsync(BidDto bid);
}
