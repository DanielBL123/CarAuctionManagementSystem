using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Common.Tests.Unitary;
using CarAuction.Common.Tests.Unitary.Extensions;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Manager.Service.Adapter;
using CarAuction.Manager.Service.Interface;
using CarAuction.Model;
using CarAuction.RealTime.Interface;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CarAuction.Manager.Service.Tests.Unitary;

public class VehicleServiceTests : BaseServiceTests<VehicleService>
{
    private readonly IVehicleTypeAdapterFactory vehicleTypeAdapterFactoryStub;
    private readonly IVehicleTypeAdapter vehicleTypeAdapterStub;
    private readonly IMapper mapperStub;

    private readonly VehicleService vehicleService;

    public VehicleServiceTests()
    {
        vehicleTypeAdapterFactoryStub = serviceCollection.AddSubstituteFor<IVehicleTypeAdapterFactory>();
        vehicleTypeAdapterStub = Substitute.For<IVehicleTypeAdapter>();
        mapperStub = serviceCollection.AddSubstituteFor<IMapper>();
        serviceCollection.AddSingleton(mapperStub);

        serviceCollection.AddTransient<VehicleService>();
        serviceCollection.AddSingleton(vehicleTypeAdapterFactoryStub);
        serviceCollection.AddSingleton(vehicleTypeAdapterStub);

        vehicleService = serviceCollection.BuildServiceProvider().GetRequiredService<VehicleService>();
    }

    [Fact]
    public async Task AddVehicle_Should_Add_Valid_Hatchback()
    {
        // Arrange
        var request = new CreateHatchbackRequest(2022, 10000, "Toyota", "Hatchback", "ABC123", 5);
        var vehicleDto = new VehicleDto();
        var vehicle = new Vehicle();

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Hatchback).Returns(vehicleTypeAdapterStub);
        vehicleTypeAdapterStub.ValidateVehicle(request).Returns((true, Enumerable.Empty<string>()));

        mapperStub.Map<VehicleDto>(request).Returns(vehicleDto);
        mapperStub.Map<Vehicle>(vehicleDto).Returns(vehicle);

        vehicleRepositoryStub.AddAsync(vehicle).Returns(vehicle);
        vehicleRepositoryStub.SaveChangesAsync().Returns(0);

        // Act
        var result = await vehicleService.AddVehicle(request);

        // Assert
        await vehicleRepositoryStub.Received(1).AddAsync(vehicle);
        await vehicleRepositoryStub.Received(1).SaveChangesAsync();
        Assert.Equal(request, result);
    }

    [Fact]
    public async Task AddVehicle_Should_Add_Valid_Sedan()
    {
        // Arrange
        var request = new CreateSedanRequest(2022, 10000, "Toyota", "Sedan", "ABC123", 5);
        var vehicleDto = new VehicleDto();
        var vehicle = new Vehicle();

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Sedan).Returns(vehicleTypeAdapterStub);
        vehicleTypeAdapterStub.ValidateVehicle(request).Returns((true, Enumerable.Empty<string>()));

        mapperStub.Map<VehicleDto>(request).Returns(vehicleDto);
        mapperStub.Map<Vehicle>(vehicleDto).Returns(vehicle);

        vehicleRepositoryStub.AddAsync(vehicle).Returns(vehicle);
        vehicleRepositoryStub.SaveChangesAsync().Returns(0);

        // Act
        var result = await vehicleService.AddVehicle(request);

        // Assert
        await vehicleRepositoryStub.Received(1).AddAsync(vehicle);
        await vehicleRepositoryStub.Received(1).SaveChangesAsync();
        Assert.Equal(request, result);
    }

    [Fact]
    public async Task AddVehicle_Should_Add_Valid_Truck()
    {
        // Arrange
        var request = new CreateTruckRequest(2022, 10000, "Toyota", "Truck", "ABC123", 100);
        var vehicleDto = new VehicleDto();
        var vehicle = new Vehicle();

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Truck).Returns(vehicleTypeAdapterStub);
        vehicleTypeAdapterStub.ValidateVehicle(request).Returns((true, Enumerable.Empty<string>()));

        mapperStub.Map<VehicleDto>(request).Returns(vehicleDto);
        mapperStub.Map<Vehicle>(vehicleDto).Returns(vehicle);

        vehicleRepositoryStub.AddAsync(vehicle).Returns(vehicle);
        vehicleRepositoryStub.SaveChangesAsync().Returns(0);

        // Act
        var result = await vehicleService.AddVehicle(request);

        // Assert
        await vehicleRepositoryStub.Received(1).AddAsync(vehicle);
        await vehicleRepositoryStub.Received(1).SaveChangesAsync();
        Assert.Equal(request, result);
    }

    [Fact]
    public async Task AddVehicle_Should_Add_Valid_Suv()
    {
        // Arrange
        var request = new CreateSuvRequest(2022, 10000, "Toyota", "Suv", "ABC123", 7);
        var vehicleDto = new VehicleDto();
        var vehicle = new Vehicle();

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Suv).Returns(vehicleTypeAdapterStub);
        vehicleTypeAdapterStub.ValidateVehicle(request).Returns((true, Enumerable.Empty<string>()));

        // Setup mapeamentos
        mapperStub.Map<VehicleDto>(request).Returns(vehicleDto);
        mapperStub.Map<Vehicle>(vehicleDto).Returns(vehicle);

        vehicleRepositoryStub.AddAsync(vehicle).Returns(vehicle);
        vehicleRepositoryStub.SaveChangesAsync().Returns(0);

        // Act
        var result = await vehicleService.AddVehicle(request);

        // Assert
        await vehicleRepositoryStub.Received(1).AddAsync(vehicle);
        await vehicleRepositoryStub.Received(1).SaveChangesAsync();
        Assert.Equal(request, result);
    }


    [Fact]
    public async Task AddVehicle_Should_Throw_When_ValidationFails()
    {
        // Arrange
        var request = new CreateHatchbackRequest(2022, 10000, "Toyota", "Corolla", "ABC123", 5);
        var errors = new[] { "Year invalid", "Model required" };

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Hatchback).Returns(vehicleTypeAdapterStub);
        vehicleTypeAdapterStub.ValidateVehicle(request).Returns((false, errors));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => vehicleService.AddVehicle(request));
        Assert.Contains("Year invalid", ex.Message);
        Assert.Contains("Model required", ex.Message);

        await vehicleRepositoryStub.DidNotReceive().AddAsync(Arg.Any<Vehicle>());
        await vehicleRepositoryStub.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task AddVehicle_Should_Throw_When_AdapterNotFound()
    {
        // Arrange
        var request = new CreateHatchbackRequest(2022, 10000, "Toyota", "Corolla", "ABC123", 5);

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Hatchback).Returns((IVehicleTypeAdapter)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => vehicleService.AddVehicle(request));
    }

    [Fact]
    public async Task AddVehicle_Should_Throw_When_EntityIsNull()
    {
        // Arrange
        var request = new CreateHatchbackRequest(2022, 10000, "Toyota", "Corolla", "ABC123", 5);
        var vehicleDto = new VehicleDto();

        vehicleTypeAdapterFactoryStub.GetService(VehicleType.Hatchback).Returns(vehicleTypeAdapterStub);
        vehicleTypeAdapterStub.ValidateVehicle(request).Returns((true, Enumerable.Empty<string>()));

        mapperStub.Map<VehicleDto>(request).Returns(vehicleDto);
        mapperStub.Map<Vehicle>(vehicleDto).Returns((Vehicle)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => vehicleService.AddVehicle(request));
    }

    [Fact]
    public async Task GetVehiclesAsync_Should_ReturnAll_When_NoFilters()
    {
        // Arrange
        var vehicles = new List<Vehicle>
        {
            new() { Id = 1, Manufacturer = "Toyota", Model = "Corolla", Year = 2022 },
            new() { Id = 2, Manufacturer = "BMW", Model = "320i", Year = 2023 }
        };

        vehicleRepositoryStub.AsQueryable().Returns(vehicles.AsQueryable());
        mapperStub.Map<IEnumerable<VehicleDto>>(Arg.Any<IEnumerable<Vehicle>>())
                    .Returns(callInfo =>
                    {
                        var source = callInfo.ArgAt<IEnumerable<Vehicle>>(0);
                        return [.. source.Select(_ => new VehicleDto())];
                    });

        // Act
        var result = await vehicleService.GetVehiclesAsync(null, null, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetVehiclesAsync_Should_FilterByManufacturer()
    {
        // Arrange
        var vehicles = new List<Vehicle>
    {
        new Vehicle { Id = 1, Manufacturer = "Toyota" },
        new Vehicle { Id = 2, Manufacturer = "BMW" }
    };

        vehicleRepositoryStub.AsQueryable().Returns(vehicles.AsQueryable());
        mapperStub.Map<IEnumerable<VehicleDto>>(Arg.Any<IEnumerable<Vehicle>>()).Returns(new List<VehicleDto> { new VehicleDto() });

        // Act
        var result = await vehicleService.GetVehiclesAsync(null, "Toyota", null, null);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetVehiclesAsync_Should_FilterByModel()
    {
        // Arrange
        var vehicles = new List<Vehicle>
    {
        new Vehicle { Id = 1, Model = "Corolla" },
        new Vehicle { Id = 2, Model = "320i" }
    };

        vehicleRepositoryStub.AsQueryable().Returns(vehicles.AsQueryable());
        mapperStub.Map<IEnumerable<VehicleDto>>(Arg.Any<IEnumerable<Vehicle>>()).Returns(new List<VehicleDto> { new VehicleDto() });

        // Act
        var result = await vehicleService.GetVehiclesAsync(null, null, "Corolla", null);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetVehiclesAsync_Should_FilterByYear()
    {
        // Arrange
        var vehicles = new List<Vehicle>
    {
        new Vehicle { Id = 1, Year = 2022 },
        new Vehicle { Id = 2, Year = 2021 }
    };

        vehicleRepositoryStub.AsQueryable().Returns(vehicles.AsQueryable());
        mapperStub.Map<IEnumerable<VehicleDto>>(Arg.Any<IEnumerable<Vehicle>>()).Returns(new List<VehicleDto> { new() });

        // Act
        var result = await vehicleService.GetVehiclesAsync(null, null, null, 2022);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetVehiclesAsync_Should_ReturnEmpty_When_NoVehicles()
    {
        // Arrange
        vehicleRepositoryStub.AsQueryable().Returns(new List<Vehicle>().AsQueryable());
        mapperStub.Map<IEnumerable<VehicleDto>>(Arg.Any<IEnumerable<Vehicle>>()).Returns(new List<VehicleDto>());

        // Act
        var result = await vehicleService.GetVehiclesAsync(null, null, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}
