using Sgcae.Domain.Common;

namespace Sgcae.Domain.AccessLogs;

public sealed class AccessLog : Entity
{
    private AccessLog()
    {
    }

    private AccessLog(
        Guid id,
        Guid? employeeId,
        string credentialCode,
        Guid accessPointId,
        MovementType movementType,
        AccessResult result,
        RejectionReason? rejectionReason,
        DateTime occurredAt)
        : base(id)
    {
        EmployeeId = employeeId;
        CredentialCode = credentialCode;
        AccessPointId = accessPointId;
        MovementType = movementType;
        Result = result;
        RejectionReason = rejectionReason;
        OccurredAt = occurredAt;
    }

    public Guid? EmployeeId { get; private set; }

    public string CredentialCode { get; private set; } = string.Empty;

    public Guid AccessPointId { get; private set; }

    public MovementType MovementType { get; private set; }

    public AccessResult Result { get; private set; }

    public RejectionReason? RejectionReason { get; private set; }

    public DateTime OccurredAt { get; private set; }

    public static AccessLog Authorized(
        Guid employeeId,
        string credentialCode,
        Guid accessPointId,
        MovementType movementType,
        DateTime occurredAt)
    {
        return new AccessLog(
            Guid.NewGuid(),
            employeeId,
            credentialCode,
            accessPointId,
            movementType,
            AccessResult.Authorized,
            null,
            occurredAt);
    }

    public static AccessLog Rejected(
        Guid? employeeId,
        string credentialCode,
        Guid accessPointId,
        MovementType movementType,
        RejectionReason reason,
        DateTime occurredAt)
    {
        return new AccessLog(
            Guid.NewGuid(),
            employeeId,
            credentialCode,
            accessPointId,
            movementType,
            AccessResult.Rejected,
            reason,
            occurredAt);
    }
}
