namespace CarAuction.Validation;
public class SuvValidator : VehicleValidator<CreateSuvRequest>
{
    public SuvValidator(IVehicleRepository vehicleRepository) : base(vehicleRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.NumberOfSeats < 5 || x.NumberOfSeats > 9).NotEmpty().WithMessage("Invalid number of seats");

    }
}
