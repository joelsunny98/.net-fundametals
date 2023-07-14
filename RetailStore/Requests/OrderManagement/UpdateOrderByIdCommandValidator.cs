using FluentValidation;
using RetailStore.Constants;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Validator for Update Order Command
/// </summary>
public class UpdateOrderByIdCommandValidator : AbstractValidator<UpdateOrderByIdCommand>
{
    /// <summary>
    /// Validator for defining specific rules for properties
    /// </summary>
    public UpdateOrderByIdCommandValidator()
    {
        //Rules for required fields
        RuleFor(x => x.OrderRequest.CustomerId).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
        RuleForEach(x => x.OrderRequest.Details).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
            p.RuleFor(x => x.Quantity).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
        });
    }
}
