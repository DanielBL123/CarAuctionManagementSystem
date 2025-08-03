using CarAuction.Common.Global.Enum;
using CarAuction.Model;

namespace CarAuction.Common.UnitTests
{
    public static class MockData
    {
        public static IList<Vehicle> MockVehicles() =>
            [
                new Vehicle(){
                    Id = 1,
                    Year = 2024,
                    StartingBid = 10000,
                    Manufacturer = "Toyota",
                    Model = "Corolla",
                    IdentificationNumber = "ABC123456",
                    VehicleType = VehicleType.Hatchback,
                    VehicleAction = VehicleAction.None,
                    IsSold = true
                },
                new Vehicle(){
                    Id = 1,
                    Year = 2024,
                    StartingBid = 20000,
                    Manufacturer = "Mercedes",
                    Model = "Amg",
                    IdentificationNumber = "ABC123488",
                    VehicleType = VehicleType.Sedan,
                    VehicleAction = VehicleAction.None,
                    IsSold = true
                },
                new Vehicle(){
                    Id = 1,
                    Year = 2018,
                    StartingBid = 15000,
                    Manufacturer = "Audi",
                    Model = "A1",
                    IdentificationNumber = "ABC123496",
                    VehicleType = VehicleType.Sedan,
                    VehicleAction = VehicleAction.None,
                    IsSold = true
                },
            ];

        public static IList<User> MockUsers() =>
            [
                new User()
                {
                    Username = "Test1",
                    PasswordHash = "Password1"
                },
                new User()
                {
                    Username = "Test2",
                    PasswordHash = "Password2"
                },
            ];

        public static IList<Auction> MockAuctions() =>
            [
                new Auction()
                {
                    Name = "TestAuction1",
                    StartDate = DateTime.Now,
                    EndDate = null,
                    Status = AuctionStatus.Active,
                    Vehicles = [MockVehicles()[0], MockVehicles()[1]]
                },
                new Auction()
                {
                    Name = "TestAuction2",
                    StartDate = DateTime.Now,
                    EndDate = null,
                    Status = AuctionStatus.Active,
                    Vehicles = [MockVehicles()[2]]
                }
            ];

        public static IList<Bid> MockBids() =>
            [
                new Bid()
                {
                    AuctionId = MockAuctions()[0].Id,
                    UserId = MockUsers()[0].Id,
                    VehicleId = MockAuctions()[0].Id,
                    Timestamp = DateTime.Now,
                    Amount = 12000,
                    User = MockUsers()[0],
                    Auction = MockAuctions()[0],
                    Vehicle = MockVehicles()[0]
                }
            ];
    }
}
