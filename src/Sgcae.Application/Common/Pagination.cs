using Sgcae.Domain.Common;

namespace Sgcae.Application.Common;

public static class Pagination
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    public static Error? Validate(int page, int pageSize)
    {
        if (page < 1)
        {
            return Error.Validation("Pagination.InvalidPage", "Page must be greater than or equal to 1.");
        }

        if (pageSize < 1 || pageSize > MaxPageSize)
        {
            return Error.Validation(
                "Pagination.InvalidPageSize",
                $"PageSize must be between 1 and {MaxPageSize}.");
        }

        return null;
    }
}
