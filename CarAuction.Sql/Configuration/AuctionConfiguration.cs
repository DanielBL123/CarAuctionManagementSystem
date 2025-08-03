namespace CarAuction.Sql.Configuration;

internal class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.StartDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(a => a.EndDate)
               .HasColumnType("datetime");

        builder.HasMany(a => a.Vehicles)
               .WithOne(v => v.Auction)
               .HasForeignKey(v => v.AuctionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Bids)
               .WithOne(b => b.Auction)
               .HasForeignKey(b => b.AuctionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
