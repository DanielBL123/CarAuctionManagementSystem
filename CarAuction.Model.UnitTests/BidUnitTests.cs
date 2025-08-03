using CarAuction.Common.UnitTests;

namespace CarAuction.Model.UnitTests;

public class BidUnitTests
{
    [Fact]
    public void Constructor_ShouldSetDefaultTimestamp()
    {
        var bid = new Bid();
        Assert.NotEqual(default, bid.Timestamp);
    }

    [Fact]
    public void Properties_ShouldSetAndGetValues()
    {
        var auction = new Auction { Id = 1 };
        var user = new User { Id = 2 };
        var vehicle = new Vehicle { Id = 3 };

        var bid = new Bid
        {
            Id = 5,
            AuctionId = auction.Id,
            UserId = user.Id,
            VehicleId = vehicle.Id,
            Amount = 15000,
            Auction = auction,
            User = user,
            Vehicle = vehicle
        };

        Assert.Equal(5, bid.Id);
        Assert.Equal(1, bid.AuctionId);
        Assert.Equal(2, bid.UserId);
        Assert.Equal(3, bid.VehicleId);
        Assert.Equal(15000, bid.Amount);
        Assert.Equal(auction, bid.Auction);
        Assert.Equal(user, bid.User);
        Assert.Equal(vehicle, bid.Vehicle);
    }

    [Fact]
    public void MockBids_ShouldReturnValidData()
    {
        var bids = MockData.MockBids();

        Assert.Single(bids);
        var bid = bids[0];

        Assert.Equal(12000, bid.Amount);
        Assert.NotNull(bid.Auction);
        Assert.NotNull(bid.Vehicle);
        Assert.NotNull(bid.User);
    }

}
