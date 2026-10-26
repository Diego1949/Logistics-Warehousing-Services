using Sgcae.Domain.Credentials;

namespace Sgcae.Application.Credentials;

public sealed record CredentialDto(
    Guid Id,
    string Code,
    Guid EmployeeId,
    CredentialStatus Status,
    DateTime IssuedAt)
{
    public static CredentialDto FromEntity(Credential credential) => new(
        credential.Id,
        credential.Code,
        credential.EmployeeId,
        credential.Status,
        credential.IssuedAt);
}
