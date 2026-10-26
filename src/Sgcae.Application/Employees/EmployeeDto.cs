using Sgcae.Domain.Employees;

namespace Sgcae.Application.Employees;

public sealed record EmployeeDto(
    Guid Id,
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    string JobTitle,
    string Department,
    EmployeeStatus Status)
{
    public static EmployeeDto FromEntity(Employee employee) => new(
        employee.Id,
        employee.EmployeeCode,
        employee.FirstName,
        employee.LastName,
        employee.Email.Value,
        employee.JobTitle,
        employee.Department,
        employee.Status);
}
