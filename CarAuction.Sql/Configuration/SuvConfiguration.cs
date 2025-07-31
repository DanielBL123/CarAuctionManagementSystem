namespace CarAuction.Sql.Configuration;

[ExcludeFromCodeCoverage]
public class SuvConfiguration : BaseVehicleEntityTypeConfiguration<Suv>
{
    public override void Configure(EntityTypeBuilder<Suv> builder)
    {
        builder.Property(pf => pf.NumberOfSeats).IsRequired();
        builder.ToTable(nameof(Suv));
    }
}
