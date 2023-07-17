using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Validator for Add Customer Command
/// </summary>
public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    /// <summary>
    /// Validator for defining specific rules for properties
    /// </summary>
    public AddCustomerCommandValidator()
    {
        RuleFor(command => command.CustomerName)
            .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
            .MaximumLength(25).WithMessage(ValidationMessage.Length);

        RuleFor(command => command.PhoneNumber)
                .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
                .Must(BeValidPhoneNumber).WithMessage(ValidationMessage.PhoneNumberLength);           
    }

    private bool BeValidPhoneNumber(long phoneNumber)
    {
        var phoneNumberString = phoneNumber.ToString();
        return phoneNumberString.Length == 10;
    }

}
