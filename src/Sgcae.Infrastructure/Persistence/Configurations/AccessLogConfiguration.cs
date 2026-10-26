using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sgcae.Domain.AccessLogs;
using Sgcae.Domain.Credentials;
using Sgcae.Domain.Employees;

namespace Sgcae.Infrastructure.Persistence.Configurations;

internal sealed class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.ToTable("AccessLogs");

        builder.HasKey(accessLog => accessLog.Id);
        builder.Property(accessLog => accessLog.Id).ValueGeneratedNever();

        builder.Property(accessLog => accessLog.CredentialCode)
            .HasMaxLength(Credential.CodeMaxLength)
            .IsRequired();

        builder.Property(accessLog => accessLog.MovementType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(accessLog => accessLog.Result)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(accessLog => accessLog.RejectionReason)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(accessLog => accessLog.OccurredAt).IsRequired();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(accessLog => accessLog.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(accessLog => accessLog.OccurredAt);
        builder.HasIndex(accessLog => accessLog.EmployeeId);
        builder.HasIndex(accessLog => accessLog.AccessPointId);
    }
}
