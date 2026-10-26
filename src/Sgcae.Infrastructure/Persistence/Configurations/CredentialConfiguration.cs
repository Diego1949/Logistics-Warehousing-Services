using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sgcae.Domain.Credentials;

namespace Sgcae.Infrastructure.Persistence.Configurations;

internal sealed class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.ToTable("Credentials");

        builder.HasKey(credential => credential.Id);
        builder.Property(credential => credential.Id).ValueGeneratedNever();

        builder.Property(credential => credential.Code)
            .HasMaxLength(Credential.CodeMaxLength)
            .IsRequired();

        builder.Property(credential => credential.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(credential => credential.IssuedAt).IsRequired();

        builder.HasIndex(credential => credential.Code).IsUnique();
    }
}
