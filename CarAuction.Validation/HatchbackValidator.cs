using CarAuction.Dto.Request;

namespace CarAuction.Validation;

public class HatchbackValidator : VehicleValidator<CreateHatchbackRequest>
{
    public HatchbackValidator(IVehicleRepository vehicleRepository) : base(vehicleRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.NumberOfDoors)
            .NotEmpty()
            .InclusiveBetween(2,5)
            .WithMessage("Invalid number of doors");

    }
}
