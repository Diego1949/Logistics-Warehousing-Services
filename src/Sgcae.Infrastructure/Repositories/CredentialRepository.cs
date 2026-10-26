using Microsoft.EntityFrameworkCore;
using Sgcae.Domain.Credentials;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Repositories;

internal sealed class CredentialRepository(SgcaeDbContext context) : ICredentialRepository
{
    public Task<Credential?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Credentials.FirstOrDefaultAsync(credential => credential.Id == id, cancellationToken);

    public Task<Credential?> GetByCodeAsync(string code, CancellationToken cancellationToken) =>
        context.Credentials.FirstOrDefaultAsync(credential => credential.Code == code, cancellationToken);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken) =>
        context.Credentials.AnyAsync(credential => credential.Code == code, cancellationToken);

    public void Add(Credential credential) => context.Credentials.Add(credential);
}
