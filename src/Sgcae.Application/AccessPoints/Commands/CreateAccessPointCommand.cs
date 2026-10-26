using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.AccessPoints.Commands;

public sealed record CreateAccessPointCommand(string Name, string? Description) : ICommand<AccessPointDto>;
