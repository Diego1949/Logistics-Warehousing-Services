using Sgcae.Domain.AccessLogs;

namespace Sgcae.Application.AccessLogs;

public sealed record AccessLogDto(
    Guid Id,
    Guid? EmployeeId,
    string CredentialCode,
    Guid AccessPointId,
    MovementType MovementType,
    AccessResult Result,
    RejectionReason? RejectionReason,
    DateTime OccurredAt)
{
    public static AccessLogDto FromEntity(AccessLog accessLog) => new(
        accessLog.Id,
        accessLog.EmployeeId,
        accessLog.CredentialCode,
        accessLog.AccessPointId,
        accessLog.MovementType,
        accessLog.Result,
        accessLog.RejectionReason,
        accessLog.OccurredAt);
}
