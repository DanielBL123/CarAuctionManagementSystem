namespace CarAuction.Validation;

public class SedanValidator : VehicleValidator<CreateSedanRequest>
{
    public SedanValidator(IVehicleRepository vehicleRepository) : base(vehicleRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.NumberOfDoors)
            .NotEmpty()
            .InclusiveBetween(5,9)
            .WithMessage("Invalid number of doors");

    }
}
