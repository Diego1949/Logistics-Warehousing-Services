using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Common;

namespace Sgcae.Application.AccessPoints.Commands;

internal sealed class CreateAccessPointCommandHandler(IAccessPointRepository accessPoints, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAccessPointCommand, AccessPointDto>
{
    public async Task<Result<AccessPointDto>> Handle(CreateAccessPointCommand command, CancellationToken cancellationToken)
    {
        var accessPointResult = AccessPoint.Create(command.Name, command.Description);
        if (accessPointResult.IsFailure)
        {
            return accessPointResult.Error;
        }

        var accessPoint = accessPointResult.Value;

        if (await accessPoints.ExistsByNameAsync(accessPoint.Name, cancellationToken))
        {
            return AccessPointErrors.DuplicateName(accessPoint.Name);
        }

        accessPoints.Add(accessPoint);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return AccessPointDto.FromEntity(accessPoint);
    }
}
