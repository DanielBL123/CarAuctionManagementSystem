
namespace CarAuction.Sql.Configuration;

public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Amount)
               .IsRequired();

        builder.Property(b => b.Timestamp)
               .HasColumnType("datetime")
               .IsRequired();

        builder.HasOne(b => b.Auction)
               .WithMany(a => a.Bids)
               .HasForeignKey(b => b.AuctionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Vehicle)
               .WithMany(v => v.Bids)
               .HasForeignKey(b => b.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.User)
               .WithMany(u => u.Bids)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
