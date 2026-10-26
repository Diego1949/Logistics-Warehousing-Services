using Microsoft.AspNetCore.Mvc;
using Sgcae.Api.Contracts;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Common;
using Sgcae.Application.Employees;
using Sgcae.Application.Employees.Commands;
using Sgcae.Application.Employees.Queries;

namespace Sgcae.Api.Controllers;

[Route("api/employees")]
public sealed class EmployeesController : ApiController
{
    [HttpPost]
    [ProducesResponseType<EmployeeDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        CreateEmployeeCommand command,
        [FromServices] ICommandHandler<CreateEmployeeCommand, EmployeeDto> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : Failure(result.Error);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<EmployeeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetEmployeeByIdQuery, EmployeeDto> handler,
        CancellationToken cancellationToken)
    {
        return OkOrFailure(await handler.Handle(new GetEmployeeByIdQuery(id), cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType<PagedResult<EmployeeDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetEmployeesQuery, PagedResult<EmployeeDto>> handler,
        CancellationToken cancellationToken,
        [FromQuery] int page = Pagination.DefaultPage,
        [FromQuery] int pageSize = Pagination.DefaultPageSize)
    {
        return OkOrFailure(await handler.Handle(new GetEmployeesQuery(page, pageSize), cancellationToken));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<EmployeeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateEmployeeRequest request,
        [FromServices] ICommandHandler<UpdateEmployeeCommand, EmployeeDto> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.JobTitle,
            request.Department);

        return OkOrFailure(await handler.Handle(command, cancellationToken));
    }

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Deactivate(
        Guid id,
        [FromServices] ICommandHandler<DeactivateEmployeeCommand> handler,
        CancellationToken cancellationToken)
    {
        return NoContentOrFailure(await handler.Handle(new DeactivateEmployeeCommand(id), cancellationToken));
    }
}
