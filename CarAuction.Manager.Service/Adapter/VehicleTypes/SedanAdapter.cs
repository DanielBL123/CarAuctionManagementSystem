using CarAuction.Dto.Request;
using FluentValidation;

namespace CarAuction.Manager.Service.Adapter.VehicleTypes;

internal class SedanAdapter(IValidator<CreateSedanRequest> validator) : BaseVehicleTypeAdapter<CreateSedanRequest>(validator), IVehicleTypeAdapter
{
    public (bool IsValid, IEnumerable<string> Errors) ValidateVehicle(object vehicle)
    {
        return Validate(vehicle as CreateSedanRequest ?? throw new ArgumentNullException(nameof(vehicle)));
    }
}
