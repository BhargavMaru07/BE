using System.Data;
using FluentValidation;
using WIMS.Application.DTOs.Warehouse;

namespace WIMS.Application.Validators.WarehouseManagement;


public class WarehouseCreateRequestValidator : AbstractValidator<WarehouseCreateRequest>
{
    public WarehouseCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MinimumLength(2).WithMessage("Warehouse name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Warehouse name must not exceed 100 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Warehouse address is required.")
            .MaximumLength(200).WithMessage("Warehouse address must not exceed 200 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Warehouse city is required.")
            .MaximumLength(50).WithMessage("Warehouse city must not exceed 50 characters.");

        RuleFor(x => x.ContactPerson)
            .NotEmpty().WithMessage("Contact person is required.")
            .MinimumLength(2).WithMessage("Contact person name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Contact person name must not exceed 100 characters.");

        RuleFor(x => x.ContactPhone)
            .NotEmpty().WithMessage("Contact phone number is required.")
            .Matches(@"^(?:\+91[\-\s]?)?[6-9]\d{9}$").WithMessage("Contact phone number must be a valid Indian phone number.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid warehouse status.");
    }
}

public class WarehouseUpdateRequestValidator : AbstractValidator<WarehouseUpdateRequest>
{
    public WarehouseUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(2).WithMessage("Warehouse name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Warehouse name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Warehouse address must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.City)
            .MaximumLength(50).WithMessage("Warehouse city must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.City));
            
        RuleFor(x => x.ContactPerson)
            .MinimumLength(2).WithMessage("Contact person name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Contact person name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactPerson));

        RuleFor(x => x.ContactPhone)
            .Matches(@"^(?:\+91[\-\s]?)?[6-9]\d{9}$").WithMessage("Contact phone number must be a valid Indian phone number.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactPhone));
    }
}

public class WarehouseStatusUpdateRequestValidator : AbstractValidator<WarehouseStatusUpdateRequest>
{
  public WarehouseStatusUpdateRequestValidator()
  {
      RuleFor(x => x.Status)
          .IsInEnum().WithMessage("Invalid warehouse status.");
  }
}