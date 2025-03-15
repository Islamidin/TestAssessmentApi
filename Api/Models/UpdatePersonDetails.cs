namespace Api.Models;

public record UpdatePersonDetails(int Id, string FirstName, string LastName, string? MiddleName, string Email, string Address, DateTime? DateOfBirth);