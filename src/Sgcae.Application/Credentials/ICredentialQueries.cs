namespace Sgcae.Application.Credentials;

public interface ICredentialQueries
{
    Task<CredentialDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<CredentialDto>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);
}
