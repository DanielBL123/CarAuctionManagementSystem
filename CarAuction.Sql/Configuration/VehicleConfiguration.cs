namespace CarAuction.Sql.Configuration;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public virtual void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Manufacturer)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(v => v.Model)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(v => v.IdentificationNumber)
               .IsRequired()
               .HasMaxLength(17);

        builder.HasIndex(v => v.IdentificationNumber)
               .IsUnique();

        builder.Property(v => v.Year)
               .IsRequired();

        builder.Property(v => v.StartingBid)
               .IsRequired();

        builder.Property(v => v.VehicleType)
               .IsRequired();

        builder.Property(v => v.VehicleAction)
               .IsRequired();

        builder.HasMany(v => v.Bids)
               .WithOne(b => b.Vehicle)
               .HasForeignKey(b => b.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
