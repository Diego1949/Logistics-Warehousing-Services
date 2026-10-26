using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.AccessLogs;
using Sgcae.Application.AccessPoints;
using Sgcae.Application.Credentials;
using Sgcae.Application.Employees;
using Sgcae.Domain.AccessLogs;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Credentials;
using Sgcae.Domain.Employees;
using Sgcae.Infrastructure.Persistence;
using Sgcae.Infrastructure.Queries;
using Sgcae.Infrastructure.Repositories;

namespace Sgcae.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SgcaeDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SgcaeDbContext>());

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<IAccessPointRepository, AccessPointRepository>();
        services.AddScoped<IAccessLogRepository, AccessLogRepository>();

        services.AddScoped<IEmployeeQueries, EmployeeQueries>();
        services.AddScoped<ICredentialQueries, CredentialQueries>();
        services.AddScoped<IAccessPointQueries, AccessPointQueries>();
        services.AddScoped<IAccessLogQueries, AccessLogQueries>();

        return services;
    }
}
