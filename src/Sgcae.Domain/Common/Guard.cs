namespace Sgcae.Domain.Common;

internal static class Guard
{
    public static Error? AgainstInvalidText(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation($"{field}.Required", $"{field} is required.");
        }

        return value.Trim().Length > maxLength
            ? Error.Validation($"{field}.TooLong", $"{field} must not exceed {maxLength} characters.")
            : null;
    }

    public static Error? AgainstTooLong(string? value, string field, int maxLength)
    {
        return value is not null && value.Trim().Length > maxLength
            ? Error.Validation($"{field}.TooLong", $"{field} must not exceed {maxLength} characters.")
            : null;
    }

    public static Error? FirstError(params Error?[] errors) => errors.FirstOrDefault(error => error is not null);
}
