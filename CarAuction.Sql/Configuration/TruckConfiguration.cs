namespace CarAuction.Sql.Configuration;

[ExcludeFromCodeCoverage]
public class TruckConfiguration : BaseVehicleEntityTypeConfiguration<Truck>
{
    public override void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder.Property(pf => pf.LoadCapacity).IsRequired();
        builder.ToTable(nameof(Truck));
    }
}
