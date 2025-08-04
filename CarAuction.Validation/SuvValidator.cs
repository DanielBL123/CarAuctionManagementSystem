namespace CarAuction.Validation;
public class SuvValidator : VehicleValidator<CreateSuvRequest>
{
    public SuvValidator(IVehicleRepository vehicleRepository) : base(vehicleRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.NumberOfSeats)
            .NotEmpty().WithMessage("Number of seats cannot be empty")
            .InclusiveBetween(5, 9).WithMessage("Invalid number of seats. Must be between 5 and 9");

    }
}
