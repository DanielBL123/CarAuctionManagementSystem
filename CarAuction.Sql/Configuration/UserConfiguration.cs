namespace CarAuction.Sql.Configuration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasMany(v => v.Vehicles)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Bids)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}