using CarAuction.Dto.Request;

namespace CarAuction.Client.Service.Interface;

public interface IBidService
{
    Task PlaceBid(CreateBidRequest request, string username);
}
