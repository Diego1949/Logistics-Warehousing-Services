using Sgcae.Domain.Common;

namespace Sgcae.Domain.Employees;

public static class EmployeeErrors
{
    public static readonly Error AlreadyInactive =
        Error.Conflict("Employee.AlreadyInactive", "The employee is already inactive.");

    public static readonly Error InactiveCannotReceiveCredential =
        Error.Conflict("Employee.Inactive", "A credential can only be assigned to an active employee.");

    public static Error NotFound(Guid id) =>
        Error.NotFound("Employee.NotFound", $"Employee with id '{id}' was not found.");

    public static Error DuplicateCode(string employeeCode) =>
        Error.Conflict("Employee.DuplicateCode", $"An employee with code '{employeeCode}' already exists.");

    public static Error DuplicateEmail(Email email) =>
        Error.Conflict("Employee.DuplicateEmail", $"An employee with email '{email}' already exists.");
}
