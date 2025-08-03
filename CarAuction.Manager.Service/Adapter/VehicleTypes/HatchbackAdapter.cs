using CarAuction.Dto.Request;
using FluentValidation;

namespace CarAuction.Manager.Service.Adapter.VehicleTypes;

internal class HatchbackAdapter(IValidator<CreateHatchbackRequest> validator) : BaseVehicleTypeAdapter<CreateHatchbackRequest>(validator), IVehicleTypeAdapter
{
    public (bool IsValid, IEnumerable<string> Errors) ValidateVehicle(object vehicle)
    {
        return Validate(vehicle as CreateHatchbackRequest ?? throw new ArgumentNullException(nameof(vehicle)));
    }
}
