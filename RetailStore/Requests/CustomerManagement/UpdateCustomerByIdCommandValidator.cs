using FluentValidation;
using RetailStore.Constants;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement;
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    private readonly RetailStoreDbContext _dbContext;

    public UpdateCustomerCommandValidator(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(command => command.CustomerId)
            .GreaterThan(0).WithMessage(ValidationMessage.Invalid);

        RuleFor(command => command.CustomerName)
            .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
            .MaximumLength(100).WithMessage(ValidationMessage.Length);

        RuleFor(command => command.PhoneNumber)
            .NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
    }
         
}
