using System.Text.RegularExpressions;
using Sgcae.Domain.Common;

namespace Sgcae.Domain.Employees;

public sealed record Email
{
    public const int MaxLength = 150;

    private static readonly Regex Pattern = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation("Email.Required", "Email is required.");
        }

        var normalized = value.Trim().ToLowerInvariant();

        if (normalized.Length > MaxLength)
        {
            return Error.Validation("Email.TooLong", $"Email must not exceed {MaxLength} characters.");
        }

        if (!Pattern.IsMatch(normalized))
        {
            return Error.Validation("Email.InvalidFormat", $"'{value}' is not a valid email address.");
        }

        return new Email(normalized);
    }

    public override string ToString() => Value;
}
