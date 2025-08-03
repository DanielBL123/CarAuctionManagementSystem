using FluentValidation;

namespace CarAuction.Manager.Service.Adapter;

internal abstract class BaseVehicleTypeAdapter<T>(IValidator<T> validator)
{
    protected virtual (bool IsValid, IEnumerable<string> Errors) Validate (T message)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(message);
            var result = validator.Validate(message);
            return (result.IsValid, result.Errors.Select(x => x.ErrorMessage));
        }
        catch(Exception ex)
        {
            return (false, new List<string>() { ex.ToString() });
        }
    }
}
