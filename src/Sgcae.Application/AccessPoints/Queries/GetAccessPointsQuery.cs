using Sgcae.Application.Abstractions.Messaging;

namespace Sgcae.Application.AccessPoints.Queries;

public sealed record GetAccessPointsQuery : IQuery<IReadOnlyList<AccessPointDto>>;
