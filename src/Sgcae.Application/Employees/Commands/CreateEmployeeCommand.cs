using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Employees.Commands;

public sealed record CreateEmployeeCommand(
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    string JobTitle,
    string Department) : ICommand<EmployeeDto>;
