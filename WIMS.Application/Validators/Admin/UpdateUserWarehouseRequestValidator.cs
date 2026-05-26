using FluentValidation;
using WIMS.Application.DTOs.Admin;

namespace WIMS.Application.Validators.Admin;
 
public class UpdateUserWarehouseRequestValidator : AbstractValidator<UpdateUserWarehouseRequest>
{
    public UpdateUserWarehouseRequestValidator()
    {
        RuleFor(x => x.WarehouseId)
            .GreaterThan(0)
            .WithMessage("Warehouse ID must be a valid positive number.");
    }
}