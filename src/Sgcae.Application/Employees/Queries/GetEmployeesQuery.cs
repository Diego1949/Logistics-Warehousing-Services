using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Common;

namespace Sgcae.Application.Employees.Queries;

public sealed record GetEmployeesQuery(int Page, int PageSize) : IQuery<PagedResult<EmployeeDto>>;
