using Sgcae.Domain.Common;
using Sgcae.Domain.Credentials;

namespace Sgcae.Domain.Employees;

public sealed class Employee : Entity
{
    public const int EmployeeCodeMaxLength = 20;
    public const int NameMaxLength = 100;
    public const int JobTitleMaxLength = 100;
    public const int DepartmentMaxLength = 100;

    private readonly List<Credential> _credentials = [];

    private Employee()
    {
    }

    private Employee(Guid id, string employeeCode)
        : base(id)
    {
        EmployeeCode = employeeCode;
        Status = EmployeeStatus.Active;
    }

    public string EmployeeCode { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public Email Email { get; private set; } = null!;

    public string JobTitle { get; private set; } = string.Empty;

    public string Department { get; private set; } = string.Empty;

    public EmployeeStatus Status { get; private set; }

    public IReadOnlyCollection<Credential> Credentials => _credentials.AsReadOnly();

    public bool IsActive => Status == EmployeeStatus.Active;

    public static Result<Employee> Create(
        string employeeCode,
        string firstName,
        string lastName,
        string email,
        string jobTitle,
        string department)
    {
        var codeError = Guard.AgainstInvalidText(employeeCode, nameof(EmployeeCode), EmployeeCodeMaxLength);
        if (codeError is not null)
        {
            return codeError;
        }

        var employee = new Employee(Guid.NewGuid(), employeeCode.Trim());
        var updateResult = employee.Update(firstName, lastName, email, jobTitle, department);

        return updateResult.IsSuccess ? employee : updateResult.Error;
    }

    public Result Update(string firstName, string lastName, string email, string jobTitle, string department)
    {
        var error = Guard.FirstError(
            Guard.AgainstInvalidText(firstName, nameof(FirstName), NameMaxLength),
            Guard.AgainstInvalidText(lastName, nameof(LastName), NameMaxLength),
            Guard.AgainstInvalidText(jobTitle, nameof(JobTitle), JobTitleMaxLength),
            Guard.AgainstInvalidText(department, nameof(Department), DepartmentMaxLength));
        if (error is not null)
        {
            return error;
        }

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = emailResult.Value;
        JobTitle = jobTitle.Trim();
        Department = department.Trim();

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (Status == EmployeeStatus.Inactive)
        {
            return EmployeeErrors.AlreadyInactive;
        }

        Status = EmployeeStatus.Inactive;

        return Result.Success();
    }

    public Result<Credential> AssignCredential(string code, DateTime issuedAt)
    {
        if (!IsActive)
        {
            return EmployeeErrors.InactiveCannotReceiveCredential;
        }

        var credentialResult = Credential.Issue(Id, code, issuedAt);
        if (credentialResult.IsFailure)
        {
            return credentialResult.Error;
        }

        _credentials.Add(credentialResult.Value);

        return credentialResult.Value;
    }
}
