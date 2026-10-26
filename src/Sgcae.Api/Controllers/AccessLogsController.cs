using Microsoft.AspNetCore.Mvc;
using Sgcae.Api.Contracts;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.AccessLogs;
using Sgcae.Application.AccessLogs.Commands;
using Sgcae.Application.AccessLogs.Queries;
using Sgcae.Application.Common;
using Sgcae.Domain.AccessLogs;

namespace Sgcae.Api.Controllers;

[Route("api/access-logs")]
public sealed class AccessLogsController : ApiController
{
    [HttpPost]
    [ProducesResponseType<AccessLogDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        RegisterAccessRequest request,
        [FromServices] ICommandHandler<RegisterAccessCommand, AccessLogDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterAccessCommand(
            request.CredentialCode,
            request.AccessPointId!.Value,
            request.MovementType!.Value);

        return OkOrFailure(await handler.Handle(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType<PagedResult<AccessLogDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHistory(
        [FromServices] IQueryHandler<GetAccessLogsQuery, PagedResult<AccessLogDto>> handler,
        CancellationToken cancellationToken,
        [FromQuery] Guid? employeeId = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] Guid? accessPointId = null,
        [FromQuery] AccessResult? result = null,
        [FromQuery] int page = Pagination.DefaultPage,
        [FromQuery] int pageSize = Pagination.DefaultPageSize)
    {
        var query = new GetAccessLogsQuery(employeeId, from, to, accessPointId, result, page, pageSize);

        return OkOrFailure(await handler.Handle(query, cancellationToken));
    }
}
