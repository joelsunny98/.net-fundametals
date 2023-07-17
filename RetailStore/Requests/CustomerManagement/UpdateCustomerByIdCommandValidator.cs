using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;

namespace RetailStore.Requests.CustomerManagement
{
    /// <summary>
    /// Validator for Update Customer Command
    /// </summary>
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {

        /// <summary>
        /// Validator for defining specific rules for properties
        /// </summary>
        public UpdateCustomerCommandValidator()
        {
            RuleFor(command => command.CustomerId)
                .GreaterThan(0).WithMessage(ValidationMessage.Invalid);

            RuleFor(command => command.CustomerName)
                .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
                .MaximumLength(25).WithMessage(ValidationMessage.Length + " (Max 25 characters)");

            RuleFor(command => command.PhoneNumber)
                .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
                .Must(BeValidPhoneNumber).WithMessage(ValidationMessage.PhoneNumberLength + " (10 digits)");
        }

        private bool BeValidPhoneNumber(long phoneNumber)
        {
            var phoneNumberString = phoneNumber.ToString();
            return phoneNumberString.Length == 10;
        }     
    }
}
