using Microsoft.AspNetCore.Mvc;
using Sgcae.Domain.Common;

namespace Sgcae.Api.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Failure(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: error.Message);
        problemDetails.Extensions["code"] = error.Code;

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }

    protected IActionResult NoContentOrFailure(Result result) =>
        result.IsSuccess ? NoContent() : Failure(result.Error);

    protected IActionResult OkOrFailure<TValue>(Result<TValue> result) =>
        result.IsSuccess ? Ok(result.Value) : Failure(result.Error);
}
