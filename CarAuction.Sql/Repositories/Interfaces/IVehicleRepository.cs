namespace CarAuction.Sql.Repositories.Interfaces;

public interface IVehicleRepository : IRepository<Vehicle, int>
{
    IEnumerable<Vehicle> GetVechilesFromAuction(int auctionId);
    Task<Vehicle?> GetLicitingVehicleByIdentificationNumber(string identificationNumber);
    Task<Vehicle?> GetVehicleByIdentificationNumber(string identificationNumber);
    void UpdateRange(IEnumerable<Vehicle> vehicles);
}
   
