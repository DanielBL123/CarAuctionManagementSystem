using Microsoft.Extensions.DependencyInjection;

namespace CarAuction.Validation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVehicleValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<HatchbackValidator>();
        return services;
    }
}
