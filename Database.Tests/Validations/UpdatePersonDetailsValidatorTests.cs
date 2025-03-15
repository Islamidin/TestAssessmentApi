using Api.Models;
using Api.Validations;
using Database.Entities;
using Database.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Tests.Validations;

[TestFixture]
public class UpdatePersonDetailsValidatorTests
{
    private const int PersonId = 1;
    private readonly CancellationToken cancellationToken = CancellationToken.None;
    private Mock<IPeopleRepository> mockPeopleRepository;
    private UpdatePersonDetailsValidator validator;

    [SetUp]
    public void SetUp()
    {
        mockPeopleRepository = new();
        validator = new(mockPeopleRepository.Object);

        mockPeopleRepository
            .Setup(repo => repo.FindByIdAsync(PersonId, cancellationToken))
            .ReturnsAsync(new Person { Id = PersonId, FirstName = "First Name", LastName = "Last", MiddleName = "Middle", Email = "email@example.com", Address = "Some Address", DateOfBirth = DateTime.Now.AddYears(-1) });
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_Person_Does_Not_Exist()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Id = PersonId + 1 };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "Person not found");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_FirstName_Is_Empty()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { FirstName = string.Empty };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName" && e.ErrorMessage == "'First Name' must not be empty.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_LastName_Is_Empty()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { LastName = string.Empty };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LastName" && e.ErrorMessage == "'Last Name' must not be empty.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_Email_Is_Empty()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Email = string.Empty };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == "'Email' must not be empty.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_Address_Is_Empty()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Address = string.Empty };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Address" && e.ErrorMessage == "'Address' must not be empty.");
    }

    [Test]
    public async Task Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails();

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_FirstName_Exceeds_MaxLength()
    {
        var longFirstName = new string('A', 101);
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { FirstName = longFirstName };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName" && e.ErrorMessage == $"The length of 'First Name' must be 100 characters or fewer. You entered {longFirstName.Length} characters.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_LastName_Exceeds_MaxLength()
    {
        var longLastName = new string('A', 101);
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { LastName = longLastName };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LastName" && e.ErrorMessage == $"The length of 'Last Name' must be 100 characters or fewer. You entered {longLastName.Length} characters.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_MiddleName_Exceeds_MaxLength()
    {
        var longMiddleName = new string('A', 101);
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { MiddleName = longMiddleName };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "MiddleName" && e.ErrorMessage == $"The length of 'Middle Name' must be 100 characters or fewer. You entered {longMiddleName.Length} characters.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_Email_Exceeds_MaxLength()
    {
        var longEmail = new string('A', 101) + "@example.com";
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Email = longEmail };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == $"The length of 'Email' must be 100 characters or fewer. You entered {longEmail.Length} characters.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_Address_Exceeds_MaxLength()
    {
        var longAddress = new string('A', 301);
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Address = longAddress };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Address" && e.ErrorMessage == $"The length of 'Address' must be 300 characters or fewer. You entered {longAddress.Length} characters.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_PersonId_Is_Invalid()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Id = 999 };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "Person not found");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_Email_Is_Invalid()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { Email = "invalid-email" };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == "'Email' is not a valid email address.");
    }

    [Test]
    public async Task Should_Have_Validation_Error_When_DateOfBirth_Is_In_Future()
    {
        var updatePersonDetails = MakeValidUpdatePersonDetails() with { DateOfBirth = DateTime.Now.AddYears(1) };

        var result = await validator.ValidateAsync(updatePersonDetails, cancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth" && e.ErrorMessage == "Date of Birth must be in the past.");
    }

    private static UpdatePersonDetails MakeValidUpdatePersonDetails() => new(PersonId, "First", "Last", "Middle", "email@example.com", "Some Address", DateTime.Now.AddYears(-1));
}