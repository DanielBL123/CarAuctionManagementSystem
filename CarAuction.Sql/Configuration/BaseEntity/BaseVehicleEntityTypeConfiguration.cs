using CarAuction.Model.BaseEntities;

namespace CarAuction.Sql.Configuration.BaseEntity;

[ExcludeFromCodeCoverage]
public abstract class BaseVehicleEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseVehicleEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(pf => pf.Id);
        builder.Property(pf => pf.Id).ValueGeneratedOnAdd();

        builder.Property(pf => pf.Manufacturer)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(pf => pf.Model)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(pf => pf.Year).IsRequired();

        builder.Property(pf => pf.StartingBid)
               .IsRequired()
               .HasColumnType("decimal(18,2)");
    }
}