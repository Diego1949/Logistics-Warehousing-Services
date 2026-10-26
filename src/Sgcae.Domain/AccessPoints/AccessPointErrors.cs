using Sgcae.Domain.Common;

namespace Sgcae.Domain.AccessPoints;

public static class AccessPointErrors
{
    public static readonly Error AlreadyEnabled =
        Error.Conflict("AccessPoint.AlreadyEnabled", "The access point is already enabled.");

    public static readonly Error AlreadyDisabled =
        Error.Conflict("AccessPoint.AlreadyDisabled", "The access point is already disabled.");

    public static Error NotFound(Guid id) =>
        Error.NotFound("AccessPoint.NotFound", $"Access point with id '{id}' was not found.");

    public static Error DuplicateName(string name) =>
        Error.Conflict("AccessPoint.DuplicateName", $"An access point named '{name}' already exists.");
}
