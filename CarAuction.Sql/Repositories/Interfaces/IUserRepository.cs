namespace CarAuction.Sql.Repositories.Interfaces;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetByUsernameAsync(string username);
}
