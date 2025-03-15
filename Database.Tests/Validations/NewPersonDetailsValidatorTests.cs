using Api.Models;
using Api.Validations;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Validations;

[TestFixture]
public class NewPersonDetailsValidatorTests
{
    private NewPersonDetailsValidator validator;

    [SetUp]
    public void SetUp()
    {
        validator = new();
    }

    [Test]
    public void Should_Have_Validation_Error_When_FirstName_Is_Empty()
    {
        var person = MakeValidNewPersonDetails() with { FirstName = string.Empty };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName" && e.ErrorMessage == "'First Name' must not be empty.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_LastName_Is_Empty()
    {
        var person = MakeValidNewPersonDetails() with { LastName = string.Empty };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LastName" && e.ErrorMessage == "'Last Name' must not be empty.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_Email_Is_Empty()
    {
        var person = MakeValidNewPersonDetails() with { Email = string.Empty };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == "'Email' must not be empty.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_Address_Is_Empty()
    {
        var person = MakeValidNewPersonDetails() with { Address = string.Empty };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Address" && e.ErrorMessage == "'Address' must not be empty.");
    }

    [Test]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var person = MakeValidNewPersonDetails();

        var result = validator.Validate(person);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Should_Have_Validation_Error_When_FirstName_Exceeds_MaxLength()
    {
        var longFirstName = new string('A', 101);
        var person = MakeValidNewPersonDetails() with { FirstName = longFirstName };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName" && e.ErrorMessage == $"The length of 'First Name' must be 100 characters or fewer. You entered {longFirstName.Length} characters.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_LastName_Exceeds_MaxLength()
    {
        var longLastName = new string('A', 101);
        var person = MakeValidNewPersonDetails() with { LastName = longLastName };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LastName" && e.ErrorMessage == $"The length of 'Last Name' must be 100 characters or fewer. You entered {longLastName.Length} characters.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_MiddleName_Exceeds_MaxLength()
    {
        var longMiddleName = new string('A', 101);
        var person = MakeValidNewPersonDetails() with { MiddleName = longMiddleName };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "MiddleName" && e.ErrorMessage == $"The length of 'Middle Name' must be 100 characters or fewer. You entered {longMiddleName.Length} characters.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_Email_Exceeds_MaxLength()
    {
        var longEmail = new string('A', 101) + "@example.com";
        var person = MakeValidNewPersonDetails() with { Email = longEmail };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == $"The length of 'Email' must be 100 characters or fewer. You entered {longEmail.Length} characters.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_Address_Exceeds_MaxLength()
    {
        var longAddress = new string('A', 301);
        var person = MakeValidNewPersonDetails() with { Address = longAddress };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Address" && e.ErrorMessage == $"The length of 'Address' must be 300 characters or fewer. You entered {longAddress.Length} characters.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_Email_Is_Invalid()
    {
        var person = MakeValidNewPersonDetails() with { Email = "invalid-email" };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == "'Email' is not a valid email address.");
    }

    [Test]
    public void Should_Have_Validation_Error_When_DateOfBirth_Is_In_Future()
    {
        var person = MakeValidNewPersonDetails() with { DateOfBirth = DateTime.Now.AddYears(1) };

        var result = validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth" && e.ErrorMessage == "Date of Birth must be in the past.");
    }

    private static NewPersonDetails MakeValidNewPersonDetails() => new("First", "Last", "Middle", "email@example.com", "Some Address", DateTime.Now.AddYears(-1));
}