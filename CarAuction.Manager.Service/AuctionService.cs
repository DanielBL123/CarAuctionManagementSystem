using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.RealTime.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarAuction.Manager.Service;

public class AuctionService(IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository, IAuctionNotifier auctionNotifier, IBidRepository bidRepository, IMapper mapper, ILogger<AuctionService> logger) : IAuctionService
{

    public async Task<AuctionDto> CreateAuctionAsync(CreateAuctionRequest createAuctionRequest)
    {
        logger.LogInformation("Creating a new auction");
        
        var auction = mapper.Map<Auction>(createAuctionRequest);

        var vehicles = GetVehiclesByIdentifierNumbers(createAuctionRequest.VehicleIdentificationNumbers);

        if (!vehicles.Any())
        {
            throw new InvalidOperationException("No vehicles found with the provided identifiers.");
        }


        if (vehicles.Any(x => x.AuctionId != null))
            throw new InvalidOperationException("There are vehicles that are already associated to otherauction.");

        await auctionRepository.AddAsync(auction);
        await auctionRepository.SaveChangesAsync();

        logger.LogInformation("Auction created successfully with Id: {Id} || Name: {Name}", auction.Id,auction.Name);

        foreach (var vehicle in vehicles)
        {
            vehicle.AuctionId = auction.Id;
            vehicleRepository.Update(vehicle);
        }

        vehicleRepository.SaveChangesAsync().GetAwaiter().GetResult();

        var auctionDto = mapper.Map<AuctionDto>(auction);

        _ = StartAuctionLifecycleAsync(auction);

            return auctionDto;
        
        
    }

    private async Task StartAuctionLifecycleAsync(Auction auction)
    {
        
        auctionNotifier.NotifyAuctionCreatedAsync(mapper.Map<AuctionDto>(auction)).GetAwaiter().GetResult();

        logger.LogInformation("Users have been notified about the new auction with Id: {Id} || Name: {Name}", auction.Id, auction.Name);
  
        foreach (var vehicle in auction.Vehicles)
        {
            vehicle.VehicleAction = VehicleAction.Liciting;
            
            vehicleRepository.Update(vehicle);
            vehicleRepository.SaveChangesAsync().GetAwaiter().GetResult();
            
            await auctionNotifier.NotifyVehicleOpenAsync(mapper.Map<VehicleDto>(vehicle));

            logger.LogInformation("Auction is now live for vehicle with Identification Number: {IdentifierNumber}", vehicle.IdentificationNumber);

            await Task.Delay(TimeSpan.FromSeconds(300)); // 5 minutes
            
            await auctionNotifier.NotifyVehicleClosedAsync(vehicle.Id);

            try
            {
                var bid = await bidRepository.AsQueryable().Where(x => x.Vehicle.IdentificationNumber.Equals(vehicle.IdentificationNumber)).Include(x => x.User).FirstOrDefaultAsync();

                var user = bid!.User;

                logger.LogInformation("Auction has finished for vehicle with Identification Number: {IdentifierNumber}. User {Username} has won the vehicle.", vehicle.IdentificationNumber, user.Username);

                vehicle.VehicleAction = user != null ? VehicleAction.Sold : VehicleAction.None;
                vehicle.User = user;
            }catch(Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            
            vehicleRepository.Update(vehicle);
            vehicleRepository.SaveChangesAsync().GetAwaiter().GetResult();

        }

        await auctionNotifier.NotifyAuctionEndedAsync(auction.Id);

        logger.LogInformation("Auction closed. Id: {Id} | Name: {Name}", auction.Id, auction.Name);

        if (auction.Status != AuctionStatus.Closed)
        {
            auction.Status = AuctionStatus.Closed;
            auction.EndDate = DateTime.UtcNow;
            auctionRepository.Update(auction);
            await auctionRepository.SaveChangesAsync();
        }
         
    }

    private IEnumerable<Vehicle> GetVehiclesByIdentifierNumbers(IEnumerable<string> vehicleIdentifiers) =>
        [.. vehicleRepository.AsQueryable(x => vehicleIdentifiers.Contains(x.IdentificationNumber))];


    public async Task<AuctionDto> CloseAuction(CloseAuctionRequest request)
    {
        var auction = await auctionRepository.GetAuctionByName(request.Name);
        ArgumentNullException.ThrowIfNull(auction);
        
        auction.EndDate = DateTime.UtcNow;
        auction.Status = AuctionStatus.Closed;


        auctionRepository.Update(auction);
        await auctionRepository.SaveChangesAsync();

        var vehiclesSold = vehicleRepository.GetVechilesFromAuction(auction.Id);

        foreach (var vehicle in vehiclesSold)
        {
            vehicle.IsSold = true;
            vehicleRepository.Update(vehicle);
        }

        await vehicleRepository.SaveChangesAsync();
        return mapper.Map<AuctionDto>(auction);
    }

    public async Task<IEnumerable<AuctionDto>> GetActiveAuctions() =>
         await Task.Run(() => mapper.Map<IEnumerable<AuctionDto>>(auctionRepository.GetAllActiveAuctions().Include(x => x.Vehicles)));

}
