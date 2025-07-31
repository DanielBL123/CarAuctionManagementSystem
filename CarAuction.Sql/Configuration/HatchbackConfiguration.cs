namespace CarAuction.Sql.Configuration;

[ExcludeFromCodeCoverage]
public class HatchbackConfiguration : BaseVehicleEntityTypeConfiguration<Hatchback>
{
    public override void Configure(EntityTypeBuilder<Hatchback> builder)
    {
        builder.Property(pf => pf.NumberOfDoors).IsRequired();
        builder.ToTable(nameof(Hatchback));
    }
}
