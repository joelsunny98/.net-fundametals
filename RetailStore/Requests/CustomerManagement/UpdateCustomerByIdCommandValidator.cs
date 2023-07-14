using FluentValidation;
using RetailStore.Constants;

namespace RetailStore.Features.CustomerManagement
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
                .MaximumLength(25).WithMessage(ValidationMessage.Length);

            RuleFor(command => command.PhoneNumber)
                .NotNull().NotEmpty().WithMessage(ValidationMessage.Required);
        }
    }
}
