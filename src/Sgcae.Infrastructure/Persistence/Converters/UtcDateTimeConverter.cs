using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sgcae.Infrastructure.Persistence.Converters;

internal sealed class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    value => value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value,
    value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
