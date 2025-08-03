using AutoMapper;
using CarAuction.Common.Tests.Unitary;
using CarAuction.Common.Tests.Unitary.Extensions;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Model;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Linq.Expressions;

namespace CarAuction.Client.Service.Tests.Unitary;

public class AuthServiceUnitTests : BaseServiceTests<AuthService>
{

    private readonly AuthService authService;
    private readonly IMapper mapperStub;


    public AuthServiceUnitTests()
    {
        mapperStub = MapperExtensions.CreateMapper<User, UserDto>();
        serviceCollection.AddSingleton(mapperStub);
        serviceCollection.AddTransient<AuthService>();
        authService = serviceCollection.BuildServiceProvider().GetRequiredService<AuthService>();
    }

    [Fact]
    public async Task LoginAsync_ReturnsUserDto_WhenCredentialsAreCorrect()
    {
        // Arrange
        var request = new LoginUserRequest("Test1", "Password1");
        var user = MockUsers().First();
        var userDto = new UserDto(1, "Test1");
        userRepositoryStub.GetByUsernameAsync("Test1").Returns(user);

        // Act
        var result = await authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto, result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LoginUserRequest("NonExisting", "AnyPassword");
        userRepositoryStub.GetByUsernameAsync("NonExisting").Returns((User)null);

        // Act
        var result = await authService.LoginAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsNull_WhenPasswordIsIncorrect()
    {
        // Arrange
        var request = new LoginUserRequest("Test1", "WrongPassword");
        var user = MockUsers().First();
        userRepositoryStub.GetByUsernameAsync("Test1").Returns(user);

        // Act
        var result = await authService.LoginAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_Throws_WhenUsernameAlreadyExists()
    {
        // Arrange
        var request = new RegisterUserRequest("Test1", "SomePassword");
        var user = MockUsers().First();
        userRepositoryStub.GetByUsernameAsync("Test1").Returns(user);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => authService.RegisterAsync(request));
    }

    [Fact]
    public async Task RegisterAsync_ReturnsUserDto_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new RegisterUserRequest("NewUser", "NewPassword");
        userRepositoryStub.GetByUsernameAsync("NewUser").Returns((User)null);

        User addedUser = null!;
        userRepositoryStub.AddAsync(Arg.Do<User>(u => addedUser = u)).Returns(addedUser);
        userRepositoryStub.SaveChangesAsync().Returns(0);

        var expectedDto = new UserDto(1, "NewUser");

        // Act
        var result = await authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result);
        Assert.Equal("NewUser", addedUser.Username);
        Assert.Equal("NewPassword", addedUser.PasswordHash);
        await userRepositoryStub.Received(1).AddAsync(Arg.Any<User>());
        await userRepositoryStub.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetVehicles_ReturnsMappedVehicles_WhenUserExists()
    {
        // Arrange
        var username = "Test1";
        var user = MockUsers().First();
        userRepositoryStub.GetByUsernameAsync(username).Returns(user);

        var vehicles = MockVehicles().Where(v => v.Id == user.Id).ToList();
        vehicleRepositoryStub.AsQueryable(Arg.Any<Expression<Func<Vehicle, bool>>>())
            .Returns(vehicles.AsQueryable());

        var vehicleDtos = vehicles.Select(v => new VehicleDto
        {
            Year = v.Year,
            StartingBid = v.StartingBid,
            Manufacturer = v.Manufacturer,
            Model = v.Model,
            IdentificationNumber = v.IdentificationNumber,
            VehicleType = v.VehicleType,
            VehicleAction = v.VehicleAction,
            IsSold = v.IsSold
        }).ToList();

        // Act
        var result = await authService.GetVehicles(username);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicleDtos.Count, result.Count());
    }

    [Fact]
    public async Task GetVehicles_ThrowsArgumentNullException_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "NotExist";
        userRepositoryStub.GetByUsernameAsync(username).Returns((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => authService.GetVehicles(username));
    }

}
