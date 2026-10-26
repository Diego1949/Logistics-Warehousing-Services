using Sgcae.Application.Common;

namespace Sgcae.Application.Employees;

public interface IEmployeeQueries
{
    Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<PagedResult<EmployeeDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
