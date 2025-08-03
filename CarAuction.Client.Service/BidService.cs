using AutoMapper;
using CarAuction.Client.Service.Interface;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.RealTime.Interface;
using CarAuction.Sql.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Client.Service;

public class BidService(IUserRepository userRepository, IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository, IBidRepository bidRepository, IBidNotifier bidNotifier, IMapper mapper) : IBidService
{
    public async Task PlaceBid(CreateBidRequest request, string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        ArgumentNullException.ThrowIfNull(user);

        var auction = await auctionRepository.AsQueryable(x => x.Name.Equals(request.AuctionName) && x.Status == AuctionStatus.Active).FirstOrDefaultAsync();
        ArgumentNullException.ThrowIfNull(auction);

        var vehicle = await vehicleRepository.GetLicitingVehicleByIdentificationNumber(request.VehicleUniqueIdentifier);
        ArgumentNullException.ThrowIfNull(vehicle);


        var lastBid = await bidRepository.AsQueryable(x => x.UserId == user!.Id! &&
                                                        x.VehicleId == vehicle!.Id! &&
                                                            x.AuctionId == auction!.Id)
                                    .OrderByDescending(x => x.Amount)
                                    .FirstOrDefaultAsync();

        var currentAmmout = lastBid?.Amount ?? vehicle.StartingBid;

        if (request.Amount < currentAmmout)
        {
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

    }
}
