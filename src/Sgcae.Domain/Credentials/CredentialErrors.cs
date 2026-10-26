using Sgcae.Domain.Common;

namespace Sgcae.Domain.Credentials;

public static class CredentialErrors
{
    public static readonly Error AlreadyBlocked =
        Error.Conflict("Credential.AlreadyBlocked", "The credential is already blocked.");

    public static readonly Error AlreadyRevoked =
        Error.Conflict("Credential.AlreadyRevoked", "The credential is already revoked.");

    public static readonly Error RevokedCannotBeBlocked =
        Error.Conflict("Credential.Revoked", "A revoked credential cannot be blocked.");

    public static Error NotFound(Guid id) =>
        Error.NotFound("Credential.NotFound", $"Credential with id '{id}' was not found.");

    public static Error DuplicateCode(string code) =>
        Error.Conflict("Credential.DuplicateCode", $"A credential with code '{code}' already exists.");
}
