using FluentValidation;
namespace RetailStore.Features.CustomerManagement.Commands;


public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(command => command.CustomerId)
            .GreaterThan(0).WithMessage("Invalid customer ID.");

        RuleFor(command => command.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.");

        RuleFor(command => command.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Must(BeValidPhoneNumber).WithMessage("Phone number must be 10 digits.");
    }

    private bool BeValidPhoneNumber(long phoneNumber)
    {
        string phoneNumberString = phoneNumber.ToString();
        return phoneNumberString.Length == 10;
    }
}

