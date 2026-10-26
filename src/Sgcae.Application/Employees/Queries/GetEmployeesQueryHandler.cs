using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Common;
using Sgcae.Domain.Common;

namespace Sgcae.Application.Employees.Queries;

internal sealed class GetEmployeesQueryHandler(IEmployeeQueries employeeQueries)
    : IQueryHandler<GetEmployeesQuery, PagedResult<EmployeeDto>>
{
    public async Task<Result<PagedResult<EmployeeDto>>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        var paginationError = Pagination.Validate(query.Page, query.PageSize);
        if (paginationError is not null)
        {
            return paginationError;
        }

        return await employeeQueries.GetPagedAsync(query.Page, query.PageSize, cancellationToken);
    }
}
