namespace Sgcae.Domain.Employees;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(string employeeCode, CancellationToken cancellationToken);

    Task<bool> ExistsByEmailAsync(Email email, Guid? excludedEmployeeId, CancellationToken cancellationToken);

    void Add(Employee employee);
}
