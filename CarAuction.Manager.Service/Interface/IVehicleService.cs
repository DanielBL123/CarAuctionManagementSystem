using CarAuction.Dto.Request;

namespace CarAuction.Manager.Service.Interface;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> SearchVehiclesAsync(IEnumerable<string>? types, string? manufacturer, string? model, int? year);

    Task<CreateVehicleRequest> AddVehicle(CreateVehicleRequest vehicle);

}
