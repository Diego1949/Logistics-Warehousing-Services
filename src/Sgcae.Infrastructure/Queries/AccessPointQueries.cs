using Microsoft.EntityFrameworkCore;
using Sgcae.Application.AccessPoints;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Queries;

internal sealed class AccessPointQueries(SgcaeDbContext context) : IAccessPointQueries
{
    public Task<AccessPointDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.AccessPoints
            .AsNoTracking()
            .Where(accessPoint => accessPoint.Id == id)
            .Select(accessPoint => new AccessPointDto(
                accessPoint.Id,
                accessPoint.Name,
                accessPoint.Description,
                accessPoint.IsEnabled))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<AccessPointDto>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.AccessPoints
            .AsNoTracking()
            .OrderBy(accessPoint => accessPoint.Name)
            .Select(accessPoint => new AccessPointDto(
                accessPoint.Id,
                accessPoint.Name,
                accessPoint.Description,
                accessPoint.IsEnabled))
            .ToListAsync(cancellationToken);
}
