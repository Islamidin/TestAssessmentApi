namespace Api.Models;

public record NewPersonDetails(string FirstName, string LastName, string? MiddleName, string Email, string Address, DateTime? DateOfBirth);