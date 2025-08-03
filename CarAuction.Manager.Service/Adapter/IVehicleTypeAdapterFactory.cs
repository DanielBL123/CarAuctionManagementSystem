using CarAuction.Common.Global.Enum;

namespace CarAuction.Manager.Service.Adapter;

public interface IVehicleTypeAdapterFactory
{
    IVehicleTypeAdapter? GetService(VehicleType vehicleType);
}
