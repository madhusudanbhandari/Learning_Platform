using FluentValidation;
using lp.Application.Dtos;
using lp.Domain.Entities;

namespace lp.Application.Validators;

public class CourseValidator : AbstractValidator<CreateCourseDto>
{
    public CourseValidator()
    {
        RuleFor(c=>c.Title)
            .NotEmpty()
            .WithMessage("Title must be provided")
            .MaximumLength(150)
            .WithMessage("Title must not exceed 100 characters");

        RuleFor(c=>c.Field)
            .NotEmpty()
            .WithMessage("Title must be provided")
            .MaximumLength(150)
            .WithMessage("Title must not exceed 100 characters");

        
        RuleFor(c=>c.Price)
            .GreaterThan(0)
            .WithMessage("Price must be positive");

        
        

        
        
        
    }
}