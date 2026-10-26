using Microsoft.EntityFrameworkCore;
using Sgcae.Application.AccessLogs;
using Sgcae.Application.AccessLogs.Queries;
using Sgcae.Application.Common;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Queries;

internal sealed class AccessLogQueries(SgcaeDbContext context) : IAccessLogQueries
{
    public async Task<PagedResult<AccessLogDto>> GetPagedAsync(GetAccessLogsQuery query, CancellationToken cancellationToken)
    {
        var accessLogs = context.AccessLogs.AsNoTracking();

        if (query.EmployeeId is not null)
        {
            accessLogs = accessLogs.Where(accessLog => accessLog.EmployeeId == query.EmployeeId);
        }

        if (query.AccessPointId is not null)
        {
            accessLogs = accessLogs.Where(accessLog => accessLog.AccessPointId == query.AccessPointId);
        }

        if (query.Result is not null)
        {
            accessLogs = accessLogs.Where(accessLog => accessLog.Result == query.Result);
        }

        if (query.From is not null)
        {
            accessLogs = accessLogs.Where(accessLog => accessLog.OccurredAt >= query.From);
        }

        if (query.To is not null)
        {
            accessLogs = accessLogs.Where(accessLog => accessLog.OccurredAt <= query.To);
        }

        var totalCount = await accessLogs.CountAsync(cancellationToken);

        var items = await accessLogs
            .OrderByDescending(accessLog => accessLog.OccurredAt)
            .ThenByDescending(accessLog => accessLog.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(accessLog => new AccessLogDto(
                accessLog.Id,
                accessLog.EmployeeId,
                accessLog.CredentialCode,
                accessLog.AccessPointId,
                accessLog.MovementType,
                accessLog.Result,
                accessLog.RejectionReason,
                accessLog.OccurredAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<AccessLogDto>(items, query.Page, query.PageSize, totalCount);
    }
}
