using CarAuction.Common.Global.Enum;
using CarAuction.RealTime.Interface;

namespace CarAuction.Client.Service;

public class BidService(IUserRepository userRepository, IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository, IBidRepository bidRepository, IBidNotifier bidNotifier, IMapper mapper, ILogger<BidService> logger) : IBidService
{
    public async Task PlaceBid(CreateBidRequest request, string username)
    {
        logger.LogInformation("Submitting a new bid from {Username}", username);

        var user = await GetUserOrThrow(username);
        var auction = await GetAuctionOrThrow(request.AuctionName);
        var vehicle = await GetVehicleOrThrow(request.VehicleUniqueIdentifier);

        var lastBid = bidRepository.AsQueryable(b =>
                            b.UserId == user.Id &&
                            b.VehicleId == vehicle.Id &&
                            b.AuctionId == auction.Id)
                        .OrderByDescending(b => b.Amount)
                        .FirstOrDefault();

        var currentAmmout = lastBid?.Amount ?? vehicle.StartingBid;

        if (request.Amount < currentAmmout)
        {
            logger.LogWarning("The bid placed by {username} for: Auction {request.AuctionName} - Ammount {request.Amount}", username, request.AuctionName, request.Amount);

            throw new InvalidOperationException("Your bid is lower than the current highest bid. Please submit a higher amount.");
        }

        var bidDto = new BidDto()
        {
            Amount = request!.Amount,
            AuctionId = auction!.Id,
            UserId = user!.Id,
            VehicleId = vehicle!.Id,
            Timestamp = DateTime.UtcNow
        };

        await bidRepository.AddAsync(mapper.Map<Bid>(bidDto));
        await bidRepository.SaveChangesAsync();

        await bidNotifier.NotifyBidPlacedAsync(bidDto);

        logger.LogInformation("The bid placed by {username} for: Auction {request.AuctionName} - Ammount {request.Amount} was accepted", username, request.AuctionName, request.Amount);

    }

    private async Task<User> GetUserOrThrow(string username)
    => await userRepository.GetByUsernameAsync(username)
       ?? throw new InvalidOperationException($"User '{username}' not found.");

    private async Task<Auction> GetAuctionOrThrow(string name)
        => await Task.Run(() => auctionRepository.AsQueryable(a => a.Name == name && a.Status == AuctionStatus.Active)
               .FirstOrDefault())
           ?? throw new InvalidOperationException($"No active auction found with name '{name}'.");

    private async Task<Vehicle> GetVehicleOrThrow(string identifier)
        => await vehicleRepository.GetLicitingVehicleByIdentificationNumber(identifier)
           ?? throw new InvalidOperationException($"Vehicle with identifier '{identifier}' not found.");

}
