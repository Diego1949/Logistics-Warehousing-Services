using Sgcae.Domain.AccessPoints;

namespace Sgcae.Application.AccessPoints;

public sealed record AccessPointDto(Guid Id, string Name, string? Description, bool IsEnabled)
{
    public static AccessPointDto FromEntity(AccessPoint accessPoint) => new(
        accessPoint.Id,
        accessPoint.Name,
        accessPoint.Description,
        accessPoint.IsEnabled);
}
