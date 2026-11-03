using FluentValidation;
using lp.Domain.Entities;

namespace lp.Application.Validator;

public class EnrollmentValidator : AbstractValidator<Enrollment>
{
    public EnrollmentValidator()
    {
        RuleFor(u=>u.StudentId)
            .GreaterThan(0)
            .WithMessage("Not a valid id");
    }
}