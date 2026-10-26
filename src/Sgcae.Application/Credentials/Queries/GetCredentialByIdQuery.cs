using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Credentials.Queries;

public sealed record GetCredentialByIdQuery(Guid CredentialId) : IQuery<CredentialDto>;
