namespace CarAuction.Validation;
public class TruckValidator : VehicleValidator<CreateTruckRequest>
{
    public TruckValidator(IVehicleRepository vehicleRepository) : base(vehicleRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.LoadCapacity)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid load capacity");
    }
}
