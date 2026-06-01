using FluentValidation;
using WIMS.Application.DTOs.Profile;

namespace WIMS.Application.Validators.Profile;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FullName)
            .MinimumLength(2).WithMessage("Full name must be at least 2 characters long.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Full name can only contain letters and spaces.")
            .MaximumLength(150).WithMessage("Full name cannot exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9\s\-\(\)]{7,20}$")
            .WithMessage("Phone number format is invalid.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
