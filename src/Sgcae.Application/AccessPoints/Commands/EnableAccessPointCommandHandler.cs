using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.AccessPoints;
using Sgcae.Domain.Common;

namespace Sgcae.Application.AccessPoints.Commands;

internal sealed class EnableAccessPointCommandHandler(IAccessPointRepository accessPoints, IUnitOfWork unitOfWork)
    : ICommandHandler<EnableAccessPointCommand>
{
    public async Task<Result> Handle(EnableAccessPointCommand command, CancellationToken cancellationToken)
    {
        var accessPoint = await accessPoints.GetByIdAsync(command.AccessPointId, cancellationToken);
        if (accessPoint is null)
        {
            return AccessPointErrors.NotFound(command.AccessPointId);
        }

        var result = accessPoint.Enable();
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
