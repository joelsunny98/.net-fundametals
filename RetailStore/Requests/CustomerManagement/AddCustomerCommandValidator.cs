using FluentValidation;
using RetailStore.Constants;

namespace RetailStore.Features.CustomerManagement
{
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
                .MaximumLength(100).WithMessage(ValidationMessage.Length);

            RuleFor(command => command.PhoneNumber).NotNull().NotEmpty().WithMessage(ValidationMessage.Required);

        }
    }
}
