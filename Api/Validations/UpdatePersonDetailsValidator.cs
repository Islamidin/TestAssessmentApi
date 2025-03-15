using Api.Models;
using Database.Repositories;
using FluentValidation;

namespace Api.Validations;

public class UpdatePersonDetailsValidator : AbstractValidator<UpdatePersonDetails>
{
    public UpdatePersonDetailsValidator(IPeopleRepository peopleRepository)
    {
        RuleFor(x => x.Id).MustAsync(async (_, personId, _, cancellationToken) =>
                          {
                              var person = await peopleRepository.FindByIdAsync(personId, cancellationToken);
                              return person != null;
                          }).WithMessage("Person not found")
                          .DependentRules(() =>
                          {
                              this.RuleForFirstName(x => x.FirstName);
                              this.RuleForLastName(x => x.LastName);
                              this.RuleForMiddleName(x => x.MiddleName);
                              this.RuleForEmail(x => x.Email);
                              this.RuleForAddress(x => x.Address);
                              this.RuleForDateOfBirth(x => x.DateOfBirth);
                          });
    }
}