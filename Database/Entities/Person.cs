using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class Person
{
    public int Id { get; set; }

    [MaxLength(100)]
    public required string FirstName { get; set; }

    [MaxLength(100)]
    public required string LastName { get; set; }

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [MaxLength(100)]
    public required string Email { get; set; }

    [MaxLength(300)]
    public required string Address { get; set; }

    public DateTime? DateOfBirth { get; set; }
}