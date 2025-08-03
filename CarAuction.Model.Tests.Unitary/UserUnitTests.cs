using CarAuction.Common.Tests.Unitary;

namespace CarAuction.Model.Tests.Unitary;

public class UserUnitTests : BaseServiceTests<UserUnitTests>
{
    [Fact]
    public void Constructor_ShouldInitializeCollections()
    {
        var user = new User();

        Assert.NotNull(user.Bids);
        Assert.NotNull(user.Vehicles);
        Assert.Empty(user.Bids);
        Assert.Empty(user.Vehicles);
    }

    [Fact]
    public void Properties_ShouldSetAndGetValues()
    {
        var user = new User
        {
            Id = 1,
            Username = "TestUser",
            PasswordHash = "Hash123"
        };

        Assert.Equal(1, user.Id);
        Assert.Equal("TestUser", user.Username);
        Assert.Equal("Hash123", user.PasswordHash);
    }

    [Fact]
    public void BidsCollection_ShouldAllowAddAndRemove()
    {
        var user = new User();
        var bid = new Bid();

        user.Bids.Add(bid);
        Assert.Single(user.Bids);

        user.Bids.Remove(bid);
        Assert.Empty(user.Bids);
    }

    [Fact]
    public void VehiclesCollection_ShouldAllowAddAndRemove()
    {
        var user = new User();
        var vehicle = new Vehicle();

        user.Vehicles.Add(vehicle);
        Assert.Single(user.Vehicles);

        user.Vehicles.Remove(vehicle);
        Assert.Empty(user.Vehicles);
    }

    [Fact]
    public void MockUsers_ShouldReturnValidData()
    {
        var users = MockUsers();

        Assert.Equal(2, users.Count);
        Assert.Equal("Test1", users[0].Username);
        Assert.Equal("Password1", users[0].PasswordHash);
        Assert.Equal("Test2", users[1].Username);
    }
}
