using System.ComponentModel.DataAnnotations;
using Sgcae.Domain.AccessLogs;

namespace Sgcae.Api.Contracts;

public sealed record RegisterAccessRequest(
    string CredentialCode,
    [property: Required] Guid? AccessPointId,
    [property: Required] MovementType? MovementType);
