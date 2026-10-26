using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.Employees.Queries;

internal sealed class GetEmployeeByIdQueryHandler(IEmployeeQueries employeeQueries)
    : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await employeeQueries.GetByIdAsync(query.EmployeeId, cancellationToken);

        return employee is null ? EmployeeErrors.NotFound(query.EmployeeId) : employee;
    }
}
