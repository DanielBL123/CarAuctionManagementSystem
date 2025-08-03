using AutoMapper;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto.Request;
using CarAuction.Manager.Service.Adapter;
using CarAuction.Model;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(IEnumerable<VehicleType>? types, string? manufacturer, string? model, int? year)
    {
        var query = vehicleRepository.AsQueryable();

        if (types != null && types.Any())
            query = query.Where(v => types.Contains(v.VehicleType));

        if (!string.IsNullOrWhiteSpace(manufacturer))
            query = query.Where(v => v.Manufacturer.Contains(manufacturer));

        if (!string.IsNullOrWhiteSpace(model))
            query = query.Where(v => v.Model.Contains(model));

        if (year.HasValue)
            query = query.Where(v => v.Year == year.Value);

        return await Task.Run(() => mapper.Map<IEnumerable<VehicleDto>>(query.ToList()));
    }

}
