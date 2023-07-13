using FluentValidation;
using RetailStore.Dtos;

namespace RetailStore.Features.OrderManagement.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Data.CustomerId).NotNull().NotEmpty();
        RuleForEach(x => x.Data.Details).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            p.RuleFor(x => x.Quantity).NotNull().NotEmpty();
        });
    }
}
