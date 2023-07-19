using FluentValidation;
using RetailStore.Constants;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Validator for Add product command
/// </summary>
public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    /// <summary>
    /// Validation rules for specific properties
    /// </summary>
    public AddProductCommandValidator()
    {
        RuleFor(command => command.ProductName).NotNull().NotEmpty()
            .WithMessage(ValidationMessage.Required);

        RuleFor(command => command.ProductPrice).NotNull().NotEmpty()
            .WithMessage(ValidationMessage.Required)
            .NotEqual(0).WithMessage(ValidationMessage.GreaterThanZero);
    }
}
