using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.Credentials.Commands;

public sealed record BlockCredentialCommand(Guid CredentialId) : ICommand;
