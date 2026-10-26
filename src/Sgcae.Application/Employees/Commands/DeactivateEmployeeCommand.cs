using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Employees.Commands;

public sealed record DeactivateEmployeeCommand(Guid EmployeeId) : ICommand;
