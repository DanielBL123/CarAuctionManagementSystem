using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.RealTime.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CarAuction.Manager.Service;

public class AuctionService(
    IServiceScopeFactory scopeFactory,
    IAuctionRepository auctionRepository,
    IVehicleRepository vehicleRepository,
    IAuctionNotifier auctionNotifier,
    IMapper mapper,
    ILogger<AuctionService> logger) : IAuctionService
{
    public async Task<AuctionDto> CreateAuctionAsync(CreateAuctionRequest createAuctionRequest)
    {
        logger.LogInformation("Creating a new auction");

        var auction = mapper.Map<Auction>(createAuctionRequest);

        var vehicles = GetVehiclesByIdentifierNumbers(createAuctionRequest.VehicleIdentificationNumbers);

        if (!vehicles.Any())
            throw new InvalidOperationException("No vehicles found with the provided identifiers.");

        if (vehicles.Any(x => x.AuctionId != null))
            throw new InvalidOperationException("There are vehicles already associated with another auction.");

        await auctionRepository.AddAsync(auction);
        await auctionRepository.SaveChangesAsync();

        logger.LogInformation("Auction created successfully with Id: {Id} || Name: {Name}", auction.Id, auction.Name);

        foreach (var vehicle in vehicles)
        {
            vehicle.AuctionId = auction.Id;
            vehicleRepository.Update(vehicle);
        }

        await vehicleRepository.SaveChangesAsync();

        _ = StartAuctionLifecycleAsync(auction.Id);

        return mapper.Map<AuctionDto>(auction);
    }

    private async Task StartAuctionLifecycleAsync(int auctionId)
    {
        using var scope = scopeFactory.CreateScope();

        var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
        var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
        var bidRepository = scope.ServiceProvider.GetRequiredService<IBidRepository>();

        var auction = await auctionRepository
            .AsQueryable(a => a.Id == auctionId)
            .Include(a => a.Vehicles)
            .FirstOrDefaultAsync();

        if (auction == null)
        {
            logger.LogWarning("Auction with Id {Id} not found for lifecycle management.", auctionId);
            return;
        }

        await auctionNotifier.NotifyAuctionCreatedAsync(mapper.Map<AuctionDto>(auction));
        logger.LogInformation("Users notified: Auction {Id} || Name: {Name}", auction.Id, auction.Name);

        foreach (var vehicle in auction.Vehicles)
        {
            vehicle.VehicleAction = VehicleAction.Liciting;
            vehicleRepository.Update(vehicle);
            await vehicleRepository.SaveChangesAsync();

            await auctionNotifier.NotifyVehicleOpenAsync(mapper.Map<VehicleDto>(vehicle));
            logger.LogInformation("Auction live for vehicle {IdentifierNumber}", vehicle.IdentificationNumber);

            await Task.Delay(TimeSpan.FromSeconds(300));

            await auctionNotifier.NotifyVehicleClosedAsync(vehicle.Id);

            try
            {
                var winningBid = await bidRepository.AsQueryable()
                    .Where(x => x.Vehicle.IdentificationNumber == vehicle.IdentificationNumber)
                    .Include(x => x.User)
                    .OrderByDescending(x => x.Amount)
                    .FirstOrDefaultAsync();

                var winner = winningBid?.User;

                logger.LogInformation(
                    "Auction finished for vehicle {IdentifierNumber}. Winner: {Username}",
                    vehicle.IdentificationNumber,
                    winner?.Username ?? "None");

                vehicle.VehicleAction = winner != null ? VehicleAction.Sold : VehicleAction.None;
                vehicle.User = winner;
                vehicleRepository.Update(vehicle);
                await vehicleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error determining winner for vehicle {IdentifierNumber}", vehicle.IdentificationNumber);
            }
        }

        await auctionNotifier.NotifyAuctionEndedAsync(auction.Id);

        if (auction.Status != AuctionStatus.Closed)
        {
            auction.Status = AuctionStatus.Closed;
            auction.EndDate = DateTime.UtcNow;
            auctionRepository.Update(auction);
            await auctionRepository.SaveChangesAsync();
        }

        logger.LogInformation("Auction closed. Id: {Id} | Name: {Name}", auction.Id, auction.Name);
    }

    private IEnumerable<Vehicle> GetVehiclesByIdentifierNumbers(IEnumerable<string> vehicleIdentifiers) =>
        vehicleRepository.AsQueryable(x => vehicleIdentifiers.Contains(x.IdentificationNumber)).ToList();

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
        mapper.Map<IEnumerable<AuctionDto>>(
            await auctionRepository.GetAllActiveAuctions()
            .Include(x => x.Vehicles)
            .ToListAsync());
}

