namespace Sgcae.Domain.Credentials;

public interface ICredentialRepository
{
    Task<Credential?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Credential?> GetByCodeAsync(string code, CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);

    void Add(Credential credential);
}
