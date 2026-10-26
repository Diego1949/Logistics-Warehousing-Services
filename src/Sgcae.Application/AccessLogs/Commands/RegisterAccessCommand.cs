using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.AccessLogs;

namespace Sgcae.Application.AccessLogs.Commands;

public sealed record RegisterAccessCommand(
    string CredentialCode,
    Guid AccessPointId,
    MovementType MovementType) : ICommand<AccessLogDto>;
