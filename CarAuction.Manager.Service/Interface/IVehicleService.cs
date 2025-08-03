using CarAuction.Common.Global.Enum;
using CarAuction.Dto.Request;

namespace CarAuction.Manager.Service.Interface;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetVehiclesAsync(IEnumerable<VehicleType>? types, string? manufacturer, string? model, int? year);

    Task<CreateVehicleRequest> AddVehicle(CreateVehicleRequest vehicle);

}
