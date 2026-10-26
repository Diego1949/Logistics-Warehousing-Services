using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Credentials;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.Credentials.Commands;

internal sealed class AssignCredentialCommandHandler(
    IEmployeeRepository employees,
    ICredentialRepository credentials,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<AssignCredentialCommand, CredentialDto>
{
    public async Task<Result<CredentialDto>> Handle(AssignCredentialCommand command, CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(command.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return EmployeeErrors.NotFound(command.EmployeeId);
        }

        var credentialResult = employee.AssignCredential(command.Code, timeProvider.GetUtcNow().UtcDateTime);
        if (credentialResult.IsFailure)
        {
            return credentialResult.Error;
        }

        var credential = credentialResult.Value;

        if (await credentials.ExistsByCodeAsync(credential.Code, cancellationToken))
        {
            return CredentialErrors.DuplicateCode(credential.Code);
        }

        credentials.Add(credential);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CredentialDto.FromEntity(credential);
    }
}
