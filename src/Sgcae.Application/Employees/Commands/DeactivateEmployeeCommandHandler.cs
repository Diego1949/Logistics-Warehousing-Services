using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.Employees.Commands;

internal sealed class DeactivateEmployeeCommandHandler(IEmployeeRepository employees, IUnitOfWork unitOfWork)
    : ICommandHandler<DeactivateEmployeeCommand>
{
    public async Task<Result> Handle(DeactivateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(command.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return EmployeeErrors.NotFound(command.EmployeeId);
        }

        var result = employee.Deactivate();
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
