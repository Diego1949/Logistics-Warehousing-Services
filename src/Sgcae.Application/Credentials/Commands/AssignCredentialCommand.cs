using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Credentials.Commands;

public sealed record AssignCredentialCommand(Guid EmployeeId, string Code) : ICommand<CredentialDto>;
