using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Common;
using Sgcae.Domain.AccessLogs;

namespace Sgcae.Application.AccessLogs.Queries;

public sealed record GetAccessLogsQuery(
    Guid? EmployeeId,
    DateTime? From,
    DateTime? To,
    Guid? AccessPointId,
    AccessResult? Result,
    int Page,
    int PageSize) : IQuery<PagedResult<AccessLogDto>>;
