using Microsoft.AspNetCore.Mvc;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.AccessPoints;
using Sgcae.Application.AccessPoints.Commands;
using Sgcae.Application.AccessPoints.Queries;

namespace Sgcae.Api.Controllers;

[Route("api/access-points")]
public sealed class AccessPointsController : ApiController
{
    [HttpPost]
    [ProducesResponseType<AccessPointDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        CreateAccessPointCommand command,
        [FromServices] ICommandHandler<CreateAccessPointCommand, AccessPointDto> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : Failure(result.Error);
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<AccessPointDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAccessPointsQuery, IReadOnlyList<AccessPointDto>> handler,
        CancellationToken cancellationToken)
    {
        return OkOrFailure(await handler.Handle(new GetAccessPointsQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<AccessPointDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetAccessPointByIdQuery, AccessPointDto> handler,
        CancellationToken cancellationToken)
    {
        return OkOrFailure(await handler.Handle(new GetAccessPointByIdQuery(id), cancellationToken));
    }

    [HttpPatch("{id:guid}/enable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Enable(
        Guid id,
        [FromServices] ICommandHandler<EnableAccessPointCommand> handler,
        CancellationToken cancellationToken)
    {
        return NoContentOrFailure(await handler.Handle(new EnableAccessPointCommand(id), cancellationToken));
    }

    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Disable(
        Guid id,
        [FromServices] ICommandHandler<DisableAccessPointCommand> handler,
        CancellationToken cancellationToken)
    {
        return NoContentOrFailure(await handler.Handle(new DisableAccessPointCommand(id), cancellationToken));
    }
}
