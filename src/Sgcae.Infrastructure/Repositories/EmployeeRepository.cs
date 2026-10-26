using Microsoft.EntityFrameworkCore;
using Sgcae.Domain.Employees;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Repositories;

internal sealed class EmployeeRepository(SgcaeDbContext context) : IEmployeeRepository
{
    public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Employees.FirstOrDefaultAsync(employee => employee.Id == id, cancellationToken);

    public Task<bool> ExistsByCodeAsync(string employeeCode, CancellationToken cancellationToken) =>
        context.Employees.AnyAsync(employee => employee.EmployeeCode == employeeCode, cancellationToken);

    public Task<bool> ExistsByEmailAsync(Email email, Guid? excludedEmployeeId, CancellationToken cancellationToken) =>
        context.Employees.AnyAsync(
            employee => employee.Email == email && employee.Id != excludedEmployeeId,
            cancellationToken);

    public void Add(Employee employee) => context.Employees.Add(employee);
}
