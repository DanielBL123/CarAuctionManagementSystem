using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto.Request;
using CarAuction.Manager.Service.Events;
using CarAuction.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarAuction.Manager.Service;

public class AuctionServiceFinalVersion(IMediator mediator, IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository, IMapper mapper) : IAuctionService
{

    public async Task<IEnumerable<AuctionDto>> GetActiveAuctions() =>
        await Task.Run(() => mapper.Map<IEnumerable<AuctionDto>>(
            auctionRepository.GetAllActiveAuctions()
            .Include(x => x.Vehicles)
            .ToList()));

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


    public async Task<AuctionDto> CreateAuctionAsync(CreateAuctionRequest request)
    {
        var auction = mapper.Map<Auction>(request);

        var vehicles = GetVehiclesByIdentifierNumbers(request.VehicleIdentificationNumbers).ToList();
        if (vehicles.Count == 0)
            throw new InvalidOperationException("No vehicles found with provided identifiers.");

        await auctionRepository.AddAsync(auction);
        await auctionRepository.SaveChangesAsync();

        foreach (var vehicle in vehicles)
        {
            vehicle.AuctionId = auction.Id;
        }

        vehicleRepository.UpdateRange(vehicles);
        await vehicleRepository.SaveChangesAsync();

        var vehicleDtos = mapper.Map<List<VehicleDto>>(vehicles);
        await mediator.Publish(new AuctionCreatedEvent(auction.Id, vehicleDtos));

        return mapper.Map<AuctionDto>(auction);
    }


    private IEnumerable<Vehicle> GetVehiclesByIdentifierNumbers(IEnumerable<string> identifiers) =>
        [.. vehicleRepository.AsQueryable(x => identifiers.Contains(x.IdentificationNumber)).AsNoTracking()];

}
