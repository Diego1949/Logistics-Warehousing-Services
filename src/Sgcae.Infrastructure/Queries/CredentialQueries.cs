using Microsoft.EntityFrameworkCore;
using Sgcae.Application.Credentials;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Queries;

internal sealed class CredentialQueries(SgcaeDbContext context) : ICredentialQueries
{
    public Task<CredentialDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Credentials
            .AsNoTracking()
            .Where(credential => credential.Id == id)
            .Select(credential => new CredentialDto(
                credential.Id,
                credential.Code,
                credential.EmployeeId,
                credential.Status,
                credential.IssuedAt))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<CredentialDto>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken) =>
        await context.Credentials
            .AsNoTracking()
            .Where(credential => credential.EmployeeId == employeeId)
            .OrderByDescending(credential => credential.IssuedAt)
            .Select(credential => new CredentialDto(
                credential.Id,
                credential.Code,
                credential.EmployeeId,
                credential.Status,
                credential.IssuedAt))
            .ToListAsync(cancellationToken);
}
