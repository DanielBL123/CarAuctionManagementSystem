using CarAuction.Dto.Request;
using FluentValidation;

namespace CarAuction.Manager.Service.Adapter.VehicleTypes;

internal class TruckAdapter(IValidator<CreateTruckRequest> validator) : BaseVehicleTypeAdapter<CreateTruckRequest>(validator), IVehicleTypeAdapter
{
    public (bool IsValid, IEnumerable<string> Errors) ValidateVehicle(object vehicle)
    {
        return Validate(vehicle as CreateTruckRequest ?? throw new ArgumentNullException(nameof(vehicle)));
    }
}
