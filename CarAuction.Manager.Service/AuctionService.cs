using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.RealTime.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Manager.Service;

public class AuctionService(IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository, IAuctionNotifier auctionNotifier, IBidRepository bidRepository, IMapper mapper) : IAuctionService
{

    public async Task<AuctionDto> CreateAuctionAsync(CreateAuctionRequest createAuctionRequest)
    {
        var auction = mapper.Map<Auction>(createAuctionRequest);

        var vehicles = GetVehiclesByIdentifierNumbers(createAuctionRequest.VehicleIdentificationNumbers);
        
        if (!vehicles.Any())
            throw new InvalidOperationException("No vehicles found with the provided identifiers.");

        if (vehicles.Any(x => x.AuctionId != null))
            throw new InvalidOperationException("There are vehicles that are already associated to other auction.");

        await auctionRepository.AddAsync(auction);
        await auctionRepository.SaveChangesAsync();

        foreach (var vehicle in vehicles)
        {
            vehicle.AuctionId = auction.Id;
            vehicleRepository.Update(vehicle);
        }

        await vehicleRepository.SaveChangesAsync();

        auction.Vehicles = [.. vehicles];

        var auctionDto = mapper.Map<AuctionDto>(auction);

        _ = StartAuctionLifecycleAsync(auction.Id);

        return auctionDto;
    }

    private async Task StartAuctionLifecycleAsync(int auctionId)
    {
        try
        {
            var auction = auctionRepository.AsQueryable(x => x.Id == auctionId)
                                            .Include(a => a.Vehicles).First();

            await auctionNotifier.NotifyAuctionCreatedAsync(mapper.Map<AuctionDto>(auction));


            if (auction == null || auction.Vehicles.Count == 0)
                return;

            foreach (var vehicle in auction.Vehicles)
            {
                vehicle.VehicleAction = VehicleAction.Liciting;
                
                vehicleRepository.Update(vehicle);
                vehicleRepository.SaveChangesAsync().GetAwaiter().GetResult();
                
                await auctionNotifier.NotifyVehicleOpenAsync(mapper.Map<VehicleDto>(vehicle));
                
                await Task.Delay(TimeSpan.FromSeconds(300)); // 5 minutes
                
                await auctionNotifier.NotifyVehicleClosedAsync(vehicle.Id);
                
                var finalUser = await bidRepository.AsQueryable().Where(x => x.Vehicle.IdentificationNumber.Equals(vehicle.IdentificationNumber)).FirstOrDefaultAsync();

                ArgumentNullException.ThrowIfNull(finalUser);

                vehicle.VehicleAction = finalUser != null ? VehicleAction.Sold: VehicleAction.None;
                vehicle.User = finalUser?.User;
                vehicleRepository.Update(vehicle);

                vehicleRepository.SaveChangesAsync().GetAwaiter().GetResult();

            }

            await auctionNotifier.NotifyAuctionEndedAsync(auctionId);

            if (auction.Status != AuctionStatus.Closed)
            {
                auction.Status = AuctionStatus.Closed;
                auction.EndDate = DateTime.UtcNow;
                auctionRepository.Update(auction);
                await auctionRepository.SaveChangesAsync();
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }    
    }

    private IEnumerable<Vehicle> GetVehiclesByIdentifierNumbers(IEnumerable<string> vehicleIdentifiers) =>
        [.. vehicleRepository.AsQueryable(x => vehicleIdentifiers.Contains(x.IdentificationNumber))];


    public async Task<AuctionDto> CloseAuction(CloseAuctionRequest request)
    {
        var auction = await auctionRepository.GetAuctionByName(request.Name);
        ArgumentNullException.ThrowIfNull(auction);
        
        // TODO: Validação
        
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

    public async Task<IEnumerable<AuctionDto>> GetActiveAuctions()
    {
        try
        {
            return await Task.Run(() => mapper.Map<IEnumerable<AuctionDto>>(auctionRepository.GetAllActiveAuctions().Include(x => x.Vehicles)));

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null;
        }
    }
    
}
