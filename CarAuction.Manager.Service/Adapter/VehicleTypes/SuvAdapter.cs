using CarAuction.Dto.Request;
using FluentValidation;

namespace CarAuction.Manager.Service.Adapter.VehicleTypes;

internal class SuvAdapter(IValidator<CreateSuvRequest> validator) : BaseVehicleTypeAdapter<CreateSuvRequest>(validator), IVehicleTypeAdapter
{
    public (bool IsValid, IEnumerable<string> Errors) ValidateVehicle(object vehicle)
    {
        return Validate(vehicle as CreateSuvRequest ?? throw new ArgumentNullException(nameof(vehicle)));
    }
}
