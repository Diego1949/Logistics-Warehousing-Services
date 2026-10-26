using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.AccessLogs;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Common;
using Sgcae.Domain.Credentials;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.AccessLogs.Commands;

internal sealed class RegisterAccessCommandHandler(
    IAccessPointRepository accessPoints,
    ICredentialRepository credentials,
    IEmployeeRepository employees,
    IAccessLogRepository accessLogs,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<RegisterAccessCommand, AccessLogDto>
{
    public async Task<Result<AccessLogDto>> Handle(RegisterAccessCommand command, CancellationToken cancellationToken)
    {
        var validationError = Validate(command);
        if (validationError is not null)
        {
            return validationError;
        }

        var credentialCode = command.CredentialCode.Trim();
        var (employeeId, rejectionReason) = await EvaluateAsync(credentialCode, command.AccessPointId, cancellationToken);
        var occurredAt = timeProvider.GetUtcNow().UtcDateTime;

        var accessLog = rejectionReason is null
            ? AccessLog.Authorized(employeeId!.Value, credentialCode, command.AccessPointId, command.MovementType, occurredAt)
            : AccessLog.Rejected(employeeId, credentialCode, command.AccessPointId, command.MovementType, rejectionReason.Value, occurredAt);

        accessLogs.Add(accessLog);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return AccessLogDto.FromEntity(accessLog);
    }

    private static Error? Validate(RegisterAccessCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.CredentialCode))
        {
            return Error.Validation("CredentialCode.Required", "CredentialCode is required.");
        }

        if (command.CredentialCode.Trim().Length > Credential.CodeMaxLength)
        {
            return Error.Validation(
                "CredentialCode.TooLong",
                $"CredentialCode must not exceed {Credential.CodeMaxLength} characters.");
        }

        if (!Enum.IsDefined(command.MovementType))
        {
            return Error.Validation("MovementType.Invalid", "MovementType must be Entry or Exit.");
        }

        return null;
    }

    private async Task<(Guid? EmployeeId, RejectionReason? RejectionReason)> EvaluateAsync(
        string credentialCode,
        Guid accessPointId,
        CancellationToken cancellationToken)
    {
        var accessPoint = await accessPoints.GetByIdAsync(accessPointId, cancellationToken);
        if (accessPoint is null)
        {
            return (null, RejectionReason.AccessPointNotFound);
        }

        if (!accessPoint.IsEnabled)
        {
            return (null, RejectionReason.AccessPointDisabled);
        }

        var credential = await credentials.GetByCodeAsync(credentialCode, cancellationToken);
        if (credential is null)
        {
            return (null, RejectionReason.CredentialNotFound);
        }

        if (!credential.CanBeUsed())
        {
            var reason = credential.Status == CredentialStatus.Revoked
                ? RejectionReason.CredentialRevoked
                : RejectionReason.CredentialBlocked;

            return (credential.EmployeeId, reason);
        }

        var employee = await employees.GetByIdAsync(credential.EmployeeId, cancellationToken);
        if (employee is null || !employee.IsActive)
        {
            return (credential.EmployeeId, RejectionReason.EmployeeInactive);
        }

        return (employee.Id, null);
    }
}
