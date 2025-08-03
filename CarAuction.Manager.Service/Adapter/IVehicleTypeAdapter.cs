namespace CarAuction.Manager.Service.Adapter;

public interface IVehicleTypeAdapter
{
    (bool IsValid, IEnumerable<string> Errors) ValidateVehicle(object vehicle);
}
