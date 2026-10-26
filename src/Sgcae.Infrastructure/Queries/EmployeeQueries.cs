using Microsoft.EntityFrameworkCore;
using Sgcae.Application.Common;
using Sgcae.Application.Employees;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Queries;

internal sealed class EmployeeQueries(SgcaeDbContext context) : IEmployeeQueries
{
    public Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Employees
            .AsNoTracking()
            .Where(employee => employee.Id == id)
            .Select(employee => new EmployeeDto(
                employee.Id,
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                employee.Email.Value,
                employee.JobTitle,
                employee.Department,
                employee.Status))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<PagedResult<EmployeeDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var employees = context.Employees.AsNoTracking();

        var totalCount = await employees.CountAsync(cancellationToken);

        var items = await employees
            .OrderBy(employee => employee.LastName)
            .ThenBy(employee => employee.FirstName)
            .ThenBy(employee => employee.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(employee => new EmployeeDto(
                employee.Id,
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                employee.Email.Value,
                employee.JobTitle,
                employee.Department,
                employee.Status))
            .ToListAsync(cancellationToken);

        return new PagedResult<EmployeeDto>(items, page, pageSize, totalCount);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) =>
        context.Employees.AnyAsync(employee => employee.Id == id, cancellationToken);
}
