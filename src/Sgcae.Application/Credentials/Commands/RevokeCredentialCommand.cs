using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Credentials.Commands;

public sealed record RevokeCredentialCommand(Guid CredentialId) : ICommand;
