using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Common;

namespace Sgcae.Application.AccessPoints.Commands;

internal sealed class DisableAccessPointCommandHandler(IAccessPointRepository accessPoints, IUnitOfWork unitOfWork)
    : ICommandHandler<DisableAccessPointCommand>
{
    public async Task<Result> Handle(DisableAccessPointCommand command, CancellationToken cancellationToken)
    {
        var accessPoint = await accessPoints.GetByIdAsync(command.AccessPointId, cancellationToken);
        if (accessPoint is null)
        {
            return AccessPointErrors.NotFound(command.AccessPointId);
        }

        var result = accessPoint.Disable();
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
