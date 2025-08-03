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

namespace CarAuction.Client.Service.Tests.Unitary;

public class BidServiceTests : BaseServiceTests<BidService>
{

    private readonly IBidNotifier bidNotifierStub;
    private readonly IMapper mapperStub;
    private readonly BidService bidService;

    public BidServiceTests()
    {
        bidNotifierStub = serviceCollection.AddSubstituteFor<IBidNotifier>();
        mapperStub = serviceCollection.AddSubstituteFor<IMapper>();
        serviceCollection.AddTransient<BidService>();

        bidService = serviceCollection.BuildServiceProvider().GetRequiredService<BidService>();
    }

    [Fact]
    public async Task PlaceBid_Should_Accept_ValidBid()
    {
        // Arrange
        var username = "Test1";
        var auctionName = "TestAuction";
        var vehicleId = "ABC123";
        var request = new CreateBidRequest(auctionName, vehicleId, 15000);

        var user = new User { Id = 1, Username = username };
        var auction = new Auction { Id = 1, Name = auctionName, Status = AuctionStatus.Active };
        var vehicle = new Vehicle { Id = 1, IdentificationNumber = vehicleId, StartingBid = 10000 };

        // Sem bids anteriores (primeiro lance)
        userRepositoryStub.GetByUsernameAsync(username).Returns(user);
        auctionRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Auction, bool>>>())
            .Returns((new List<Auction> { auction }).AsQueryable());
        vehicleRepositoryStub.GetLicitingVehicleByIdentificationNumber(vehicleId).Returns(vehicle);
        bidRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Bid, bool>>>())
            .Returns((new List<Bid>()).AsQueryable());


        // Act
        await bidService.PlaceBid(request, username);

        // Assert
        await bidRepositoryStub.Received(1).AddAsync(Arg.Any<Bid>());
        await bidRepositoryStub.Received(1).SaveChangesAsync();
        await bidNotifierStub.Received(1).NotifyBidPlacedAsync(Arg.Is<BidDto>(b =>
            b.Amount == request.Amount &&
            b.UserId == user.Id &&
            b.AuctionId == auction.Id &&
            b.VehicleId == vehicle.Id
        ));
    }

    [Fact]
    public async Task PlaceBid_Should_Throw_WhenBidIsLowerThanCurrent()
    {
        // Arrange
        var username = "Test1";
        var auctionName = "TestAuction";
        var vehicleId = "ABC123";
        var request = new CreateBidRequest(auctionName, vehicleId, 8000);

        var user = new User { Id = 1, Username = username };
        var auction = new Auction { Id = 1, Name = auctionName, Status = AuctionStatus.Active };
        var vehicle = new Vehicle { Id = 1, IdentificationNumber = vehicleId, StartingBid = 10000 };

        var lastBid = new Bid { Amount = 12000, UserId = 1, VehicleId = 1, AuctionId = 1 };

        userRepositoryStub.GetByUsernameAsync(username).Returns(user);
        auctionRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Auction, bool>>>())
            .Returns((new List<Auction> { auction }).AsQueryable());
        vehicleRepositoryStub.GetLicitingVehicleByIdentificationNumber(vehicleId).Returns(vehicle);
        bidRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Bid, bool>>>())
            .Returns((new List<Bid> { lastBid }).AsQueryable());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => bidService.PlaceBid(request, username));
        Assert.Equal("Your bid is lower than the current highest bid. Please submit a higher amount.", ex.Message);

    }

    [Fact]
    public async Task PlaceBid_Should_Throw_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new CreateBidRequest("A", "V", 10000);
        userRepositoryStub.GetByUsernameAsync(Arg.Any<string>()).Returns((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => bidService.PlaceBid(request, "unknownUser"));
    }

    [Fact]
    public async Task PlaceBid_Should_Throw_WhenAuctionDoesNotExist()
    {
        // Arrange
        var username = "Test1";
        var request = new CreateBidRequest("NotExist", "V", 10000);
        userRepositoryStub.GetByUsernameAsync(username).Returns(new User { Id = 1, Username = username });
        auctionRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Auction, bool>>>()).Returns(Enumerable.Empty<Auction>().AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => bidService.PlaceBid(request, username));
    }

    [Fact]
    public async Task PlaceBid_Should_Throw_WhenVehicleDoesNotExist()
    {
        // Arrange
        var username = "Test1";
        var auctionName = "TestAuction";
        var request = new CreateBidRequest(auctionName, "NotExist", 10000);
        userRepositoryStub.GetByUsernameAsync(username).Returns(new User { Id = 1, Username = username });
        auctionRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Auction, bool>>>()).Returns((new List<Auction> { new Auction { Id = 1, Name = auctionName, Status = AuctionStatus.Active } }).AsQueryable());
        vehicleRepositoryStub.GetLicitingVehicleByIdentificationNumber(request.VehicleUniqueIdentifier).Returns((Vehicle)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => bidService.PlaceBid(request, username));
    }
}
