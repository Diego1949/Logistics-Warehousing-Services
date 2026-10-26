namespace Sgcae.Domain.AccessPoints;

public interface IAccessPointRepository
{
    Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);

    void Add(AccessPoint accessPoint);
}
