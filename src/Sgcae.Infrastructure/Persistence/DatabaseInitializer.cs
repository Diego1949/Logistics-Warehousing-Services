using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sgcae.Domain.AccessPoints;

namespace Sgcae.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    private static readonly (string Name, string Description)[] DefaultAccessPoints =
    [
        ("Reception", "Main building reception"),
        ("Parking", "Employee parking lot"),
        ("Warehouse", "Storage and dispatch warehouse"),
        ("Restricted Area", "Restricted area for authorized personnel")
    ];

    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SgcaeDbContext>();

        await context.Database.MigrateAsync();

        if (await context.AccessPoints.AnyAsync())
        {
            return;
        }

        foreach (var (name, description) in DefaultAccessPoints)
        {
            context.AccessPoints.Add(AccessPoint.Create(name, description).Value);
        }

        await context.SaveChangesAsync();
    }
}
