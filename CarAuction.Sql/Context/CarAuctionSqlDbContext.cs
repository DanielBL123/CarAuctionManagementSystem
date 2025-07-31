using CarAuction.Model;
using CarAuction.Model.BaseEntities;
using System.Diagnostics.CodeAnalysis;

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

        public DbSet<BaseVehicleEntity> BaseVehicleEntity { get; set; }
        public DbSet<Hatchback> Hatchback { get; set; }
        public DbSet<Sudan> Sudan { get; set; }
        public DbSet<Suv> Suv { get; set; }
        public DbSet<Truck> Truck { get; set; }
    }
}
