using Sgcae.Domain.Common;

namespace Sgcae.Domain.AccessPoints;

public sealed class AccessPoint : Entity
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 250;

    private AccessPoint()
    {
    }

    private AccessPoint(Guid id, string name, string? description)
        : base(id)
    {
        Name = name;
        Description = description;
        IsEnabled = true;
    }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsEnabled { get; private set; }

    public static Result<AccessPoint> Create(string name, string? description)
    {
        var error = Guard.FirstError(
            Guard.AgainstInvalidText(name, nameof(Name), NameMaxLength),
            Guard.AgainstTooLong(description, nameof(Description), DescriptionMaxLength));
        if (error is not null)
        {
            return error;
        }

        var normalizedDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

        return new AccessPoint(Guid.NewGuid(), name.Trim(), normalizedDescription);
    }

    public Result Enable()
    {
        if (IsEnabled)
        {
            return AccessPointErrors.AlreadyEnabled;
        }

        IsEnabled = true;

        return Result.Success();
    }

    public Result Disable()
    {
        if (!IsEnabled)
        {
            return AccessPointErrors.AlreadyDisabled;
        }

        IsEnabled = false;

        return Result.Success();
    }
}
