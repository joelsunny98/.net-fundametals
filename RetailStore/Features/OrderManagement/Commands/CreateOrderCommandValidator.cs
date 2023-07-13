using FluentValidation;
using RetailStore.Constants;
using RetailStore.Dtos;

namespace RetailStore.Features.OrderManagement.Commands;

/// <summary>
/// Validator for Create Order Command
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    /// <summary>
    /// Validator for defining specific rules for properties
    /// </summary>
    public CreateOrderCommandValidator()
    {
        //Rules for required fields
        RuleFor(x => x.Data.CustomerId).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
        RuleForEach(x => x.Data.Details).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
            p.RuleFor(x => x.Quantity).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
        });
    }
}
