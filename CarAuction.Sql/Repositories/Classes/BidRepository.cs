namespace CarAuction.Sql.Repositories.Classes;

public class BidRepository(CarAuctionSqlDbContext dbContext) : Repository<Bid, int, CarAuctionSqlDbContext>(dbContext), IBidRepository
{

}
