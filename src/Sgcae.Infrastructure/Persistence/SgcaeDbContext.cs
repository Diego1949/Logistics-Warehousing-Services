using Microsoft.EntityFrameworkCore;
using Sgcae.Application.Abstractions.Data;
using Sgcae.Domain.AccessLogs;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Credentials;
using Sgcae.Domain.Employees;
using Sgcae.Infrastructure.Persistence.Converters;

namespace Sgcae.Infrastructure.Persistence;

public sealed class SgcaeDbContext(DbContextOptions<SgcaeDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Credential> Credentials => Set<Credential>();

    public DbSet<AccessPoint> AccessPoints => Set<AccessPoint>();

    public DbSet<AccessLog> AccessLogs => Set<AccessLog>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SgcaeDbContext).Assembly);
    }
}
