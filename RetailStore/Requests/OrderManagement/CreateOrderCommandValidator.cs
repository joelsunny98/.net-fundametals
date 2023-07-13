using FluentValidation;
using RetailStore.Constants;
using RetailStore.Dtos;

namespace RetailStore.Requests.OrderManagement;

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
        RuleFor(x => x.CustomerId).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
        RuleForEach(x => x.Details).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
            p.RuleFor(x => x.Quantity).Must(x => x != 0).WithMessage(ValidationMessage.Required);
        });
    }
}
