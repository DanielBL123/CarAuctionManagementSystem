

using CarAuction.Common.Global.Enum;
using System.Linq;

namespace CarAuction.Sql.Repositories.Classes;

public class AuctionRepository(CarAuctionSqlDbContext dbContext) : Repository<Auction, int, CarAuctionSqlDbContext>(dbContext), IAuctionRepository
{
    public IQueryable<Auction>GetAllActiveAuctions() => AsQueryable(x => x.Status != AuctionStatus.Active);
    public Task<Auction?> GetAuctionByName(string name) => AsQueryable(x => x.Name.Equals(name)).FirstOrDefaultAsync();
}
