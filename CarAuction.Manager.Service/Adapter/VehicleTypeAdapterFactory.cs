using CarAuction.Common.Global.Enum;
using CarAuction.Manager.Service.Adapter.VehicleTypes;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuction.Manager.Service.Adapter;

public class VehicleTypeAdapterFactory(IServiceProvider serviceProvider) : IVehicleTypeAdapterFactory
{
    public IVehicleTypeAdapter? GetService(VehicleType vehicleType)
    {

        return vehicleType switch
        {
            var _ when vehicleType == VehicleType.Hatchback => serviceProvider.GetService<HatchbackAdapter>(),
            var _ when vehicleType == VehicleType.Sedan => serviceProvider.GetService<SedanAdapter>(),
            var _ when vehicleType == VehicleType.Suv => serviceProvider.GetService<SuvAdapter>(),
            var _ when vehicleType == VehicleType.Truck => serviceProvider.GetService<TruckAdapter>(),
            _ => throw new InvalidOperationException("Invalid vehicle type")
        };
    }
}
