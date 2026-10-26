using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.AccessPoints.Commands;

public sealed record EnableAccessPointCommand(Guid AccessPointId) : ICommand;
