namespace Sgcae.Domain.AccessLogs;

public enum RejectionReason
{
    CredentialNotFound,
    CredentialBlocked,
    CredentialRevoked,
    EmployeeInactive,
    AccessPointNotFound,
    AccessPointDisabled
}
