namespace CarAuction.Sql.Repositories.Classes;

public class UserRepository(CarAuctionSqlDbContext context) : Repository<User, int, CarAuctionSqlDbContext>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }
}
