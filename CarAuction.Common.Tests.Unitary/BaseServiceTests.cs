using AutoMapper;
using CarAuction.Client.Service.Interface;
using CarAuction.Common.Global.Enum;
using CarAuction.Common.Tests.Unitary.Extensions;
using CarAuction.Manager.Service.Interface;
using CarAuction.Model;
using CarAuction.Sql.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace CarAuction.Common.Tests.Unitary
{
    [ExcludeFromCodeCoverage]
    public class BaseServiceTests<T>
    {
        protected readonly IAuthService authServiceStub;
        protected readonly IBidService bidServiceStub;
        protected readonly IAuctionService auctionServiceStub;
        protected readonly IVehicleService vehicleServiceStub;
        
        protected readonly IUserRepository userRepositoryStub;
        protected readonly IVehicleRepository vehicleRepositoryStub;
        protected readonly IBidRepository bidRepositoryStub;
        protected readonly IAuctionRepository auctionRepositoryStub;
        
        protected readonly ILogger<T> loggerStub;

        protected ServiceCollection serviceCollection;

        protected BaseServiceTests()
        {
            serviceCollection = new ServiceCollection();

            loggerStub = serviceCollection.AddSubstituteFor<ILogger<T>>();

            userRepositoryStub = serviceCollection.AddSubstituteFor<IUserRepository>();
            vehicleRepositoryStub = serviceCollection.AddSubstituteFor<IVehicleRepository>();
            bidRepositoryStub = serviceCollection.AddSubstituteFor<IBidRepository>();
            auctionRepositoryStub = serviceCollection.AddSubstituteFor<IAuctionRepository>();
            

            authServiceStub = serviceCollection.AddSubstituteFor<IAuthService>();
            bidServiceStub = serviceCollection.AddSubstituteFor<IBidService>();
            auctionServiceStub = serviceCollection.AddSubstituteFor<IAuctionService>();
            vehicleServiceStub = serviceCollection.AddSubstituteFor<IVehicleService>();

            
        }

        protected static IList<Vehicle> MockVehicles() =>
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

        protected static IList<User> MockUsers() =>
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

        protected static IList<Auction> MockAuctions() =>
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

        protected static IList<Bid> MockBids() =>
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
