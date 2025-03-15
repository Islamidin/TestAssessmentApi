using Api.Models;
using FluentValidation;

namespace Api.Validations;

public class NewPersonDetailsValidator : AbstractValidator<NewPersonDetails>
{
    public NewPersonDetailsValidator()
    {
        this.RuleForFirstName(x => x.FirstName);
        this.RuleForLastName(x => x.LastName);
        this.RuleForMiddleName(x => x.MiddleName);
        this.RuleForEmail(x => x.Email);
        this.RuleForAddress(x => x.Address);
        this.RuleForDateOfBirth(x => x.DateOfBirth);
    }
}