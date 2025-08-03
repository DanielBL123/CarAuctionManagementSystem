using System.Linq.Expressions;

namespace CarAuction.Sql.Repositories.Interfaces;

public interface IAuctionRepository : IRepository<Auction, int>
{
    Task<Auction?> GetAuctionByName(string name);
    IQueryable<Auction> GetAllActiveAuctions();
}
