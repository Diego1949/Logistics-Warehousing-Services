using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;

namespace Sgcae.Application.AccessPoints.Queries;

internal sealed class GetAccessPointsQueryHandler(IAccessPointQueries accessPointQueries)
    : IQueryHandler<GetAccessPointsQuery, IReadOnlyList<AccessPointDto>>
{
    public async Task<Result<IReadOnlyList<AccessPointDto>>> Handle(
        GetAccessPointsQuery query,
        CancellationToken cancellationToken)
    {
        var accessPoints = await accessPointQueries.GetAllAsync(cancellationToken);

        return Result.Success(accessPoints);
    }
}
