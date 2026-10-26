using Microsoft.AspNetCore.Mvc;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Credentials;
using Sgcae.Application.Credentials.Commands;
using Sgcae.Application.Credentials.Queries;

namespace Sgcae.Api.Controllers;

[Route("api/credentials")]
public sealed class CredentialsController : ApiController
{
    [HttpPost]
    [ProducesResponseType<CredentialDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Assign(
        AssignCredentialCommand command,
        [FromServices] ICommandHandler<AssignCredentialCommand, CredentialDto> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : Failure(result.Error);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<CredentialDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetCredentialByIdQuery, CredentialDto> handler,
        CancellationToken cancellationToken)
    {
        return OkOrFailure(await handler.Handle(new GetCredentialByIdQuery(id), cancellationToken));
    }

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType<IReadOnlyList<CredentialDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromServices] IQueryHandler<GetCredentialsByEmployeeQuery, IReadOnlyList<CredentialDto>> handler,
        CancellationToken cancellationToken)
    {
        return OkOrFailure(await handler.Handle(new GetCredentialsByEmployeeQuery(employeeId), cancellationToken));
    }

    [HttpPatch("{id:guid}/block")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Block(
        Guid id,
        [FromServices] ICommandHandler<BlockCredentialCommand> handler,
        CancellationToken cancellationToken)
    {
        return NoContentOrFailure(await handler.Handle(new BlockCredentialCommand(id), cancellationToken));
    }

    [HttpPatch("{id:guid}/revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Revoke(
        Guid id,
        [FromServices] ICommandHandler<RevokeCredentialCommand> handler,
        CancellationToken cancellationToken)
    {
        return NoContentOrFailure(await handler.Handle(new RevokeCredentialCommand(id), cancellationToken));
    }
}
