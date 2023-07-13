using FluentValidation;

namespace RetailStore.Features.CustomerManagement.Commands;
public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(command => command.CustomerId)
            .GreaterThan(0).WithMessage("Invalid customer ID.");
    }
}
