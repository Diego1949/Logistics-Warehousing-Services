using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.AccessPoints.Queries;

public sealed record GetAccessPointByIdQuery(Guid AccessPointId) : IQuery<AccessPointDto>;
