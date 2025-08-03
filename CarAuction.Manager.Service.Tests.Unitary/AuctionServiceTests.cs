using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Common.Tests.Unitary;
using CarAuction.Common.Tests.Unitary.Extensions;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.RealTime.Interface;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Linq.Expressions;

namespace CarAuction.Manager.Service.Tests.Unitary;

public class AuctionServiceTests : BaseServiceTests<AuctionService>
{

    private readonly IMapper mapperStub;
    private readonly IAuctionNotifier auctionNotifier;
    private readonly AuctionService auctionService;

    public AuctionServiceTests()
    {
        mapperStub = serviceCollection.AddSubstituteFor<IMapper>();
        serviceCollection.AddSubstituteFor<IAuctionNotifier>();
        serviceCollection.AddTransient<AuctionService>();
        
        auctionService = serviceCollection.BuildServiceProvider().GetRequiredService<AuctionService>();
    }

    [Fact]
    public async Task CreateAuctionAsync_Should_CreateAuction_With_ValidVehicles()
    {
        // Arrange
        var request = new CreateAuctionRequest("LeilaoTeste", DateTime.UtcNow, null, ["V1", "V2"]);

        var vehicles = new List<Vehicle>
        {
            new Vehicle { Id = 1, IdentificationNumber = "V1", AuctionId = null },
            new Vehicle { Id = 2, IdentificationNumber = "V2", AuctionId = null }
        };

        var auction = new Auction { Id = 123, Name = "LeilaoTeste", Vehicles = vehicles };

        mapperStub.Map<Auction>(request).Returns(auction);
        vehicleRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Vehicle, bool>>>())
            .Returns(vehicles.AsQueryable());
        auctionRepositoryStub.AddAsync(auction).Returns(auction);
        auctionRepositoryStub.SaveChangesAsync().Returns(0);
        vehicleRepositoryStub.Update(Arg.Any<Vehicle>());
        vehicleRepositoryStub.SaveChangesAsync().Returns(0);
        mapperStub.Map<AuctionDto>(auction).Returns(new AuctionDto { Id = 123, Name = "LeilaoTeste", Vehicles = [] });

        // Act
        var result = await auctionService.CreateAuctionAsync(request);

        // Assert
        await auctionRepositoryStub.Received(1).AddAsync(auction);
        await auctionRepositoryStub.Received(1).SaveChangesAsync();
        vehicleRepositoryStub.Received(2).Update(Arg.Any<Vehicle>());
        await vehicleRepositoryStub.Received(1).SaveChangesAsync();
        Assert.NotNull(result);
        Assert.Equal("LeilaoTeste", result.Name);
    }

    [Fact]
    public async Task CreateAuctionAsync_Should_Throw_When_NoVehiclesFound()
    {
        // Arrange
        var request = new CreateAuctionRequest("LeilaoTeste", DateTime.UtcNow, null, ["V1", "V2"]);


        mapperStub.Map<Auction>(request).Returns(new Auction());
        vehicleRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Vehicle, bool>>>())
            .Returns((new List<Vehicle>()).AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            auctionService.CreateAuctionAsync(request));
    }

    [Fact]
    public async Task CreateAuctionAsync_Should_Throw_When_Vehicle_AlreadyAssociated()
    {
        // Arrange
        var request = new CreateAuctionRequest("LeilaoTeste", DateTime.UtcNow, null, ["V1", "V2"]);

        var vehicles = new List<Vehicle>
        {
            new Vehicle { IdentificationNumber = "V1", AuctionId = 10 },
            new Vehicle { IdentificationNumber = "V2", AuctionId = null }
        };

        mapperStub.Map<Auction>(request).Returns(new Auction());
        vehicleRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Vehicle, bool>>>())
            .Returns(vehicles.AsQueryable());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            auctionService.CreateAuctionAsync(request));
    }

    [Fact]
    public async Task CloseAuction_Should_SetStatusClosed_And_MarkVehiclesSold()
    {
        // Arrange
        var auction = new Auction { Id = 1, Name = "LeilaoTeste", Status = AuctionStatus.Active };
        var vehicles = new List<Vehicle> {
        new Vehicle { Id = 1, AuctionId = 1, IsSold = false },
        new Vehicle { Id = 2, AuctionId = 1, IsSold = false }
    };
        var request = new CloseAuctionRequest("LeilaoTeste");

        auctionRepositoryStub.GetAuctionByName("LeilaoTeste").Returns(Task.FromResult(auction));
        vehicleRepositoryStub.GetVechilesFromAuction(1).Returns(vehicles);
        mapperStub.Map<AuctionDto>(auction).Returns(new AuctionDto { Id = 1, Name = "LeilaoTeste" });

        auctionRepositoryStub.SaveChangesAsync().Returns(0);
        vehicleRepositoryStub.SaveChangesAsync().Returns(0);

        // Act
        var result = await auctionService.CloseAuction(request);

        // Assert
        Assert.Equal(AuctionStatus.Closed, auction.Status);
        Assert.All(vehicles, v => Assert.True(v.IsSold));
        auctionRepositoryStub.Received(1).Update(auction);
        await auctionRepositoryStub.Received(1).SaveChangesAsync();
        vehicleRepositoryStub.Received(2).Update(Arg.Any<Vehicle>());
        await vehicleRepositoryStub.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CloseAuction_Should_Throw_If_Auction_NotFound()
    {
        // Arrange
        var request = new CloseAuctionRequest ("Inexistente");
        auctionRepositoryStub.GetAuctionByName(request.Name).Returns(Task.FromResult<Auction>(null));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => auctionService.CloseAuction(request));
    }

    [Fact]
    public async Task GetActiveAuctions_Should_Return_List()
    {
        // Arrange
        var auctions = new List<Auction> { new() { Id = 1, Name = "LeilaoTeste", Vehicles = new List<Vehicle>() } };
        auctionRepositoryStub.GetAllActiveAuctions().Returns(auctions.AsQueryable());
        mapperStub.Map<IEnumerable<AuctionDto>>(Arg.Any<IQueryable<Auction>>())
            .Returns([new AuctionDto { Id = 1, Name = "LeilaoTeste" }]);

        // Act
        var result = await auctionService.GetActiveAuctions();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("LeilaoTeste", result.First().Name);
    }

    [Fact]
    public async Task GetActiveAuctions_Should_ReturnNull_OnException()
    {
        // Arrange
        auctionRepositoryStub.GetAllActiveAuctions().Returns(x => throw new Exception("DB error"));

        // Act
        var result = await auctionService.GetActiveAuctions();

        // Assert
        Assert.Null(result);
    }

}
