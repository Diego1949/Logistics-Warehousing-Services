using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Common;

namespace Sgcae.Application.AccessPoints.Queries;

internal sealed class GetAccessPointByIdQueryHandler(IAccessPointQueries accessPointQueries)
    : IQueryHandler<GetAccessPointByIdQuery, AccessPointDto>
{
    public async Task<Result<AccessPointDto>> Handle(GetAccessPointByIdQuery query, CancellationToken cancellationToken)
    {
        var accessPoint = await accessPointQueries.GetByIdAsync(query.AccessPointId, cancellationToken);

        return accessPoint is null ? AccessPointErrors.NotFound(query.AccessPointId) : accessPoint;
    }
}
