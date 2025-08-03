using CarAuction.Common.Global.Enum;
using CarAuction.Common.UnitTests;

namespace CarAuction.Model.UnitTests;

public class AuctionTests
{
    
    [Fact]
    public void Constructor_ShouldInitializeCollectionsAndDefaults()
    {
        var auction = new Auction();
    
        Assert.NotNull(auction.Vehicles);
        Assert.NotNull(auction.Bids);
        Assert.Empty(auction.Vehicles);
        Assert.Empty(auction.Bids);
        Assert.Equal(AuctionStatus.Active, auction.Status);
    }
    
    [Fact]
    public void Properties_ShouldSetAndGetValues()
    {
        var auction = new Auction
        {
            Id = 1,
            Name = "AuctionTest",
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 1, 10),
            Status = AuctionStatus.Active
        };
    
        Assert.Equal(1, auction.Id);
        Assert.Equal("AuctionTest", auction.Name);
        Assert.Equal(new DateTime(2024, 1, 1), auction.StartDate);
        Assert.Equal(new DateTime(2024, 1, 10), auction.EndDate);
        Assert.Equal(AuctionStatus.Active, auction.Status);
    }
    
    [Fact]
    public void VehiclesCollection_ShouldAllowAddAndRemove()
    {
        var auction = new Auction();
        var vehicle = MockData.MockVehicles().First();
    
        auction.Vehicles.Add(vehicle);
        Assert.Single(auction.Vehicles);
    
        auction.Vehicles.Remove(vehicle);
        Assert.Empty(auction.Vehicles);
    }
    
    [Fact]
    public void BidsCollection_ShouldAllowAddAndRemove()
    {
        var auction = new Auction();
        var bid = new Bid();
    
        auction.Bids.Add(bid);
        Assert.Single(auction.Bids);
    
        auction.Bids.Remove(bid);
        Assert.Empty(auction.Bids);
    }
    
    [Fact]
    public void MockAuctions_ShouldReturnValidData()
    {
        var auctions = MockData.MockAuctions();
    
        Assert.Equal(2, auctions.Count);
        Assert.Equal("TestAuction1", auctions[0].Name);
        Assert.True(auctions.All(a => a.Status == AuctionStatus.Active));
        Assert.NotEmpty(auctions[0].Vehicles);
        Assert.Single(auctions[1].Vehicles);
    }
    
}


