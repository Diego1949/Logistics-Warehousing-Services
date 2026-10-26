using Sgcae.Application.Abstractions.Data;
using Sgcae.Application.Abstractions.Messaging;
using Sgcae.Domain.Common;
using Sgcae.Domain.Credentials;

namespace Sgcae.Application.Credentials.Commands;

internal sealed class BlockCredentialCommandHandler(ICredentialRepository credentials, IUnitOfWork unitOfWork)
    : ICommandHandler<BlockCredentialCommand>
{
    public async Task<Result> Handle(BlockCredentialCommand command, CancellationToken cancellationToken)
    {
        var credential = await credentials.GetByIdAsync(command.CredentialId, cancellationToken);
        if (credential is null)
        {
            return CredentialErrors.NotFound(command.CredentialId);
        }

        var result = credential.Block();
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
