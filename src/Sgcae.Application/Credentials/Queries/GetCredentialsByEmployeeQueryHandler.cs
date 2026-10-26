using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Application.Employees;
using Sgcae.Domain.Common;
using Sgcae.Domain.Employees;

namespace Sgcae.Application.Credentials.Queries;

internal sealed class GetCredentialsByEmployeeQueryHandler(
    IEmployeeQueries employeeQueries,
    ICredentialQueries credentialQueries)
    : IQueryHandler<GetCredentialsByEmployeeQuery, IReadOnlyList<CredentialDto>>
{
    public async Task<Result<IReadOnlyList<CredentialDto>>> Handle(
        GetCredentialsByEmployeeQuery query,
        CancellationToken cancellationToken)
    {
        if (!await employeeQueries.ExistsAsync(query.EmployeeId, cancellationToken))
        {
            return EmployeeErrors.NotFound(query.EmployeeId);
        }

        var credentials = await credentialQueries.GetByEmployeeIdAsync(query.EmployeeId, cancellationToken);

        return Result.Success(credentials);
    }
}
