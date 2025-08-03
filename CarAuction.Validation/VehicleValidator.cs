namespace CarAuction.Validation
    {
        public class VehicleValidator<T> : AbstractValidator<T> where T : CreateVehicleRequest
        {
            private readonly IVehicleRepository _vehicleRepository;
            public VehicleValidator(IVehicleRepository vehicleRepository)
            {
                RuleFor(x => x.Manufacturer)
                    .NotEmpty().WithMessage("Manufacturer is required.")
                    .MaximumLength(100);

                RuleFor(x => x.Model)
                    .NotEmpty().WithMessage("Model is required.")
                    .MaximumLength(50);

                _vehicleRepository = vehicleRepository;

                RuleFor(x => x.IdentificationNumber)
                    .NotEmpty().WithMessage("Identification Number is required.")
                    .MaximumLength(17).WithMessage("Identification Number have a maximum of 17 characters.")
                    .NotEmpty()
                    .Must(i => !CheckIfIndentificationNumberAlreadyExists(i))
                                    .WithName("IdentificationNumber")
                                    .WithMessage("There is already a vehicle created with that identification number");
                
                RuleFor(x => x.Year)
                    .GreaterThan(1900).LessThanOrEqualTo(DateTime.Now.Year)
                    .WithMessage("Year must be realistic.");
               
            }

            protected bool CheckIfIndentificationNumberAlreadyExists(string identificationNumber) =>
                _vehicleRepository.AsQueryable(x => 
                    x.IdentificationNumber.ToLower().Equals(identificationNumber.ToLower())).Any();
        }
    }
