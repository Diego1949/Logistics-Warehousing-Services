using Microsoft.EntityFrameworkCore;
using Sgcae.Domain.AccessPoints;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Repositories;

internal sealed class AccessPointRepository(SgcaeDbContext context) : IAccessPointRepository
{
    public Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.AccessPoints.FirstOrDefaultAsync(accessPoint => accessPoint.Id == id, cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken) =>
        context.AccessPoints.AnyAsync(accessPoint => accessPoint.Name == name, cancellationToken);

    public void Add(AccessPoint accessPoint) => context.AccessPoints.Add(accessPoint);
}
