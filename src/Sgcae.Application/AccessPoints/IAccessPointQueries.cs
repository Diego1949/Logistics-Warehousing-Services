namespace Sgcae.Application.AccessPoints;

public interface IAccessPointQueries
{
    Task<AccessPointDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<AccessPointDto>> GetAllAsync(CancellationToken cancellationToken);
}
