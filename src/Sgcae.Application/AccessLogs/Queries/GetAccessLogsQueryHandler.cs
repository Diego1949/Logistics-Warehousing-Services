using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Common;
using Sgcae.Domain.Common;

namespace Sgcae.Application.AccessLogs.Queries;

internal sealed class GetAccessLogsQueryHandler(IAccessLogQueries accessLogQueries)
    : IQueryHandler<GetAccessLogsQuery, PagedResult<AccessLogDto>>
{
    public async Task<Result<PagedResult<AccessLogDto>>> Handle(GetAccessLogsQuery query, CancellationToken cancellationToken)
    {
        var paginationError = Pagination.Validate(query.Page, query.PageSize);
        if (paginationError is not null)
        {
            return paginationError;
        }

        if (query.From > query.To)
        {
            return Error.Validation("DateRange.Invalid", "From must be earlier than or equal to To.");
        }

        return await accessLogQueries.GetPagedAsync(query, cancellationToken);
    }
}
