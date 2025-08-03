using CarAuction.Dto.Request;

namespace CarAuction.Manager.Service.Interface;

public interface IAuctionService
{
    public Task<AuctionDto> CreateAuctionAsync(CreateAuctionRequest request);
    public Task<AuctionDto> CloseAuction(CloseAuctionRequest request);
    public Task<IEnumerable<AuctionDto>> GetActiveAuctions();

}
