using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Credentials;

namespace Sgcae.Application.Credentials.Queries;

internal sealed class GetCredentialByIdQueryHandler(ICredentialQueries credentialQueries)
    : IQueryHandler<GetCredentialByIdQuery, CredentialDto>
{
    public async Task<Result<CredentialDto>> Handle(GetCredentialByIdQuery query, CancellationToken cancellationToken)
    {
        var credential = await credentialQueries.GetByIdAsync(query.CredentialId, cancellationToken);

        return credential is null ? CredentialErrors.NotFound(query.CredentialId) : credential;
    }
}
