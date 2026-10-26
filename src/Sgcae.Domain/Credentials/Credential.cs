using Sgcae.Domain.Common;

namespace Sgcae.Domain.Credentials;

public sealed class Credential : Entity
{
    public const int CodeMaxLength = 50;

    private Credential()
    {
    }

    private Credential(Guid id, Guid employeeId, string code, DateTime issuedAt)
        : base(id)
    {
        EmployeeId = employeeId;
        Code = code;
        IssuedAt = issuedAt;
        Status = CredentialStatus.Active;
    }

    public string Code { get; private set; } = string.Empty;

    public Guid EmployeeId { get; private set; }

    public CredentialStatus Status { get; private set; }

    public DateTime IssuedAt { get; private set; }

    internal static Result<Credential> Issue(Guid employeeId, string code, DateTime issuedAt)
    {
        var codeError = Guard.AgainstInvalidText(code, nameof(Code), CodeMaxLength);
        if (codeError is not null)
        {
            return codeError;
        }

        return new Credential(Guid.NewGuid(), employeeId, code.Trim(), issuedAt);
    }

    public bool CanBeUsed() => Status == CredentialStatus.Active;

    public Result Block()
    {
        if (Status == CredentialStatus.Revoked)
        {
            return CredentialErrors.RevokedCannotBeBlocked;
        }

        if (Status == CredentialStatus.Blocked)
        {
            return CredentialErrors.AlreadyBlocked;
        }

        Status = CredentialStatus.Blocked;

        return Result.Success();
    }

    public Result Revoke()
    {
        if (Status == CredentialStatus.Revoked)
        {
            return CredentialErrors.AlreadyRevoked;
        }

        Status = CredentialStatus.Revoked;

        return Result.Success();
    }
}
