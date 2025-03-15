using System.Linq.Expressions;
using FluentValidation;

namespace Api.Validations;

public static class PersonCommonValidations
{
    public static IRuleBuilderOptions<T, string> RuleForFirstName<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> firstNameAccessor) =>
        validator.RuleFor(firstNameAccessor)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty()
                 .MaximumLength(100);

    public static IRuleBuilderOptions<T, string> RuleForLastName<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> lastNameAccessor) =>
        validator.RuleFor(lastNameAccessor)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty()
                 .MaximumLength(100);

    public static IRuleBuilderOptions<T, string?> RuleForMiddleName<T>(this AbstractValidator<T> validator, Expression<Func<T, string?>> middleNameAccessor) =>
        validator.RuleFor(middleNameAccessor)
                 .Cascade(CascadeMode.Stop)
                 .MaximumLength(100);

    public static IRuleBuilderOptions<T, string> RuleForEmail<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> emailAccessor) =>
        validator.RuleFor(emailAccessor)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty()
                 .MaximumLength(100)
                 .EmailAddress();

    public static IRuleBuilderOptions<T, string> RuleForAddress<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> addressNameAccessor) =>
        validator.RuleFor(addressNameAccessor)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty()
                 .MaximumLength(300);

    public static IRuleBuilderOptions<T, DateTime?> RuleForDateOfBirth<T>(this AbstractValidator<T> validator, Expression<Func<T, DateTime?>> dateOfBirthAccessor) =>
        validator.RuleFor(dateOfBirthAccessor)
                 .LessThanOrEqualTo(DateTime.Now)
                 .WithMessage("Date of Birth must be in the past.");
}