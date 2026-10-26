namespace Sgcae.Api.Contracts;

public sealed record UpdateEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string JobTitle,
    string Department);
