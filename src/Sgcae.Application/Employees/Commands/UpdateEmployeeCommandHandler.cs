using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.Employees.Commands;

internal sealed class UpdateEmployeeCommandHandler(IEmployeeRepository employees, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateEmployeeCommand, EmployeeDto>
{
    public async Task<Result<EmployeeDto>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(command.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return EmployeeErrors.NotFound(command.EmployeeId);
        }

        var updateResult = employee.Update(
            command.FirstName,
            command.LastName,
            command.Email,
            command.JobTitle,
            command.Department);
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        if (await employees.ExistsByEmailAsync(employee.Email, employee.Id, cancellationToken))
        {
            return EmployeeErrors.DuplicateEmail(employee.Email);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return EmployeeDto.FromEntity(employee);
    }
}
