

using CarAuction.Common.Global.Enum;
using CarAuction.Model;

namespace CarAuction.Sql.Repositories.Classes;

public class VehicleRepository(CarAuctionSqlDbContext dbContext) : Repository<Vehicle, int, CarAuctionSqlDbContext>(dbContext), IVehicleRepository
{
    public IEnumerable<Vehicle> GetVechilesFromAuction(int auctionId) =>
            AsQueryable(x => x.AuctionId == auctionId)
                .Include(x => x.Auction);

    public Task<Vehicle?> GetLicitingVehicleByIdentificationNumber(string identificationNumber) =>
            AsQueryable(x => x.IdentificationNumber.Equals(identificationNumber) && x.VehicleAction == VehicleAction.Liciting).FirstOrDefaultAsync();
}
