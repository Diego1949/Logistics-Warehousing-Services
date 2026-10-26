using Sgcae.Application.AccessLogs.Queries;
using Sgcae.Application.Common;

namespace Sgcae.Application.AccessLogs;

public interface IAccessLogQueries
{
    Task<PagedResult<AccessLogDto>> GetPagedAsync(GetAccessLogsQuery query, CancellationToken cancellationToken);
}
