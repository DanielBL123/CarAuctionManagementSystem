namespace CarAuction.Sql.Configuration;

[ExcludeFromCodeCoverage]
public class SudanConfiguration : BaseVehicleEntityTypeConfiguration<Sudan>
{
    public override void Configure(EntityTypeBuilder<Sudan> builder)
    {
        builder.Property(pf => pf.NumberOfDoors).IsRequired();
        builder.ToTable(nameof(Sudan));
    }
}
