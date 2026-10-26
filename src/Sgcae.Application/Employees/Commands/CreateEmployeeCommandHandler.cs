using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.Employees.Commands;

internal sealed class CreateEmployeeCommandHandler(IEmployeeRepository employees, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<Result<EmployeeDto>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeResult = Employee.Create(
            command.EmployeeCode,
            command.FirstName,
            command.LastName,
            command.Email,
            command.JobTitle,
            command.Department);
        if (employeeResult.IsFailure)
        {
            return employeeResult.Error;
        }

        var employee = employeeResult.Value;

        if (await employees.ExistsByCodeAsync(employee.EmployeeCode, cancellationToken))
        {
            return EmployeeErrors.DuplicateCode(employee.EmployeeCode);
        }

        if (await employees.ExistsByEmailAsync(employee.Email, null, cancellationToken))
        {
            return EmployeeErrors.DuplicateEmail(employee.Email);
        }

        employees.Add(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return EmployeeDto.FromEntity(employee);
    }
}
