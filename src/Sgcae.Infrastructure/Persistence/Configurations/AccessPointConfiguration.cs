using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sgcae.Domain.AccessPoints;

namespace Sgcae.Infrastructure.Persistence.Configurations;

internal sealed class AccessPointConfiguration : IEntityTypeConfiguration<AccessPoint>
{
    public void Configure(EntityTypeBuilder<AccessPoint> builder)
    {
        builder.ToTable("AccessPoints");

        builder.HasKey(accessPoint => accessPoint.Id);
        builder.Property(accessPoint => accessPoint.Id).ValueGeneratedNever();

        builder.Property(accessPoint => accessPoint.Name)
            .HasMaxLength(AccessPoint.NameMaxLength)
            .IsRequired();

        builder.Property(accessPoint => accessPoint.Description)
            .HasMaxLength(AccessPoint.DescriptionMaxLength);

        builder.Property(accessPoint => accessPoint.IsEnabled).IsRequired();

        builder.HasIndex(accessPoint => accessPoint.Name).IsUnique();
    }
}
