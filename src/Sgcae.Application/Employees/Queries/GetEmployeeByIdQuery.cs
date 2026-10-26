using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Employees.Queries;

public sealed record GetEmployeeByIdQuery(Guid EmployeeId) : IQuery<EmployeeDto>;
