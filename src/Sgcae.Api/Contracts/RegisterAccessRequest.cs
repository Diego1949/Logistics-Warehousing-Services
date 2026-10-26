using System.ComponentModel.DataAnnotations;
using Sgcae.Domain.AccessLogs;

namespace Sgcae.Api.Contracts;

public sealed record RegisterAccessRequest(
    string CredentialCode,
    [Required] Guid? AccessPointId,
    [Required] MovementType? MovementType);
