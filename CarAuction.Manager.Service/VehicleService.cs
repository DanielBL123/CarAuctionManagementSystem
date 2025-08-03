using AutoMapper;
using CarAuction.Dto.Request;
using CarAuction.Manager.Service.Adapter;
using CarAuction.Model;

namespace CarAuction.Manager.Service;

public class VehicleService(IVehicleRepository vehicleRepository, IVehicleTypeAdapterFactory vehicleTypeAdapterFactory, IMapper mapper) : IVehicleService
{
    public async Task<CreateVehicleRequest> AddVehicle(CreateVehicleRequest createVehicleRequest)
    {
        IVehicleTypeAdapter vehicleTypeAdapter = vehicleTypeAdapterFactory.GetService(createVehicleRequest.VehicleType) ?? throw new ArgumentException(nameof(vehicleTypeAdapterFactory));
        
        var (IsValid, Errors) = vehicleTypeAdapter.ValidateVehicle(createVehicleRequest);

        if (IsValid)
        {
            var entity = mapper.Map<Vehicle>(mapper.Map<VehicleDto>(createVehicleRequest));
            ArgumentNullException.ThrowIfNull(entity);

            await vehicleRepository.AddAsync(entity);
            await vehicleRepository.SaveChangesAsync();

            return createVehicleRequest;
        }

        throw new InvalidOperationException(string.Join(",", Errors.ToArray()));

        
    }

    public Task<IEnumerable<VehicleDto>> SearchVehiclesAsync(IEnumerable<string>? types, string? manufacturer, string? model, int? year)
    {
        return null;
    }

    //public Task<HatchbackDto> AddHatchbackAsync(HatchbackDto dto) =>
    //    AddVehicleAsync<HatchbackDto, Hatchback>(dto, hatchbackRepository.AddAsync, vehicleRepository.SaveChangesAsync);

    //public Task<SedanDto> AddSedanAsync(SedanDto dto) =>
    //    AddVehicleAsync<SedanDto, Sedan>(dto, sedanRepository.AddAsync, sedanRepository.SaveChangesAsync);

    //public Task<SuvDto> AddSuvAsync(SuvDto dto) =>
    //    AddVehicleAsync<SuvDto, Suv>(dto, suvRepository.AddAsync, suvRepository.SaveChangesAsync);

    //public Task<TruckDto> AddTruckAsync(TruckDto dto) =>
    //    AddVehicleAsync<TruckDto, Truck>(dto, truckRepository.AddAsync, truckRepository.SaveChangesAsync);


    //private async Task<TDto> AddVehicleAsync<TDto, TEntity>(TDto dto, Func<TEntity, Task> addFunc, Func<Task> saveChangesFunc) where TEntity : Vehicle
    //{
    //    var entity = mapper.Map<TEntity>(dto);
    //    ArgumentNullException.ThrowIfNull(entity);

    //    await addFunc(entity);
    //    await saveChangesFunc();

    //    return dto;
    //}

}
