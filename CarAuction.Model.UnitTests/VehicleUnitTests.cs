using CarAuction.Common.Global.Enum;
using CarAuction.Common.UnitTests;
using CarAuction.Model;

namespace CarAuction.Tests.Model;

public class VehicleTests
{
    [Fact]
    public void Constructor_ShouldInitializeDefaults()
    {
        // Arrange & Act
        var vehicle = new Vehicle();

        // Assert
        Assert.NotNull(vehicle.Bids);
        Assert.Empty(vehicle.Bids);
        Assert.Equal(VehicleAction.None, vehicle.VehicleAction);
        Assert.False(vehicle.IsSold);
        Assert.Null(vehicle.Auction);
        Assert.Null(vehicle.User);
    }

    [Fact]
    public void Properties_ShouldSetAndGetValues()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            // Act
            Id = 1,
            Year = 2024,
            StartingBid = 10000,
            Manufacturer = "Toyota",
            Model = "Corolla",
            IdentificationNumber = "VIN123",
            VehicleType = VehicleType.Hatchback,
            VehicleAction = VehicleAction.Sold,
            NumberOfDoors = 4,
            NumberOfSeats = 5,
            LoadCapacity = 200.5,
            IsSold = true
        };

        // Assert
        Assert.Equal(1, vehicle.Id);
        Assert.Equal(2024, vehicle.Year);
        Assert.Equal(10000, vehicle.StartingBid);
        Assert.Equal("Toyota", vehicle.Manufacturer);
        Assert.Equal("Corolla", vehicle.Model);
        Assert.Equal("VIN123", vehicle.IdentificationNumber);
        Assert.Equal(VehicleType.Hatchback, vehicle.VehicleType);
        Assert.Equal(VehicleAction.Sold, vehicle.VehicleAction);
        Assert.Equal(4, vehicle.NumberOfDoors);
        Assert.Equal(5, vehicle.NumberOfSeats);
        Assert.Equal(200.5, vehicle.LoadCapacity);
        Assert.True(vehicle.IsSold);
    }

    [Fact]
    public void Collection_Bids_ShouldAllowAddAndRemove()
    {
        // Arrange
        var vehicle = new Vehicle();
        var bid = new Bid();

        // Act
        vehicle.Bids.Add(bid);

        // Assert Add
        Assert.Single(vehicle.Bids);
        Assert.Contains(bid, vehicle.Bids);

        // Act Remove
        vehicle.Bids.Remove(bid);

        // Assert Remove
        Assert.Empty(vehicle.Bids);
    }

    [Fact]
    public void MockVehicles_ShouldReturnValidData()
    {
        // Arrange
        var vehicles = MockData.MockVehicles();

        // Assert
        Assert.Equal(3, vehicles.Count);
        Assert.Equal("Toyota", vehicles[0].Manufacturer);
        Assert.Equal("Mercedes", vehicles[1].Manufacturer);
        Assert.Equal("Audi", vehicles[2].Manufacturer);
        Assert.All(vehicles, v => Assert.True(v.IsSold));
    }
}

