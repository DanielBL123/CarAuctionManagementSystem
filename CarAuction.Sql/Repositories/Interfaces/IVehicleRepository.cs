namespace CarAuction.Sql.Repositories.Interfaces;

public interface IVehicleRepository : IRepository<Vehicle, int>
{
    IEnumerable<Vehicle> GetVechilesFromAuction(int auctionId);
    Task<Vehicle?> GetLicitingVehicleByIdentificationNumber(string identificationNumber);
}
