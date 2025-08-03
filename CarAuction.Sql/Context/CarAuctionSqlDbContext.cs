namespace CarAuction.Sql.Context
{
    [ExcludeFromCodeCoverage]
    public class CarAuctionSqlDbContext(DbContextOptions<CarAuctionSqlDbContext> options) : DbContext (options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CarAuctionSqlDbContext).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Auction> Auction { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Bid> Bid { get; set; }

    }
}
