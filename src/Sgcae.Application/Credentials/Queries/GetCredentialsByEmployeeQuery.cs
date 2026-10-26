using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Credentials.Queries;

public sealed record GetCredentialsByEmployeeQuery(Guid EmployeeId) : IQuery<IReadOnlyList<CredentialDto>>;
