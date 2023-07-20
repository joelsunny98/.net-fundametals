using FluentValidation;
using RetailStore.Constants;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement
{
    /// <summary>
    /// Validator for Add Customer Command
    /// </summary>
    public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
    {
        private readonly RetailStoreDbContext _dbContext;

        /// <summary>
        /// Validator for defining specific rules for properties
        /// </summary>
        public AddCustomerCommandValidator(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.CustomerName)
                .NotNull().NotEmpty().WithMessage(command => string.Format(ValidationMessage.Required, "CustomerName"))
                .MaximumLength(25).WithMessage(command => string.Format(ValidationMessage.Length, "CustomerName"));

            RuleFor(command => command.PhoneNumber)
                .NotNull().NotEmpty().WithMessage(command => string.Format(ValidationMessage.Required, "Phone Number"))
                .Must(BeValidPhoneNumber).WithMessage(command => string.Format(ValidationMessage.PhoneNumberLength, "Phone Number"))
                .Must(BeUniquePhoneNumber).WithMessage(command => string.Format(ValidationMessage.Unique, "Phone Number"));
        }

        private bool BeValidPhoneNumber(long phoneNumber)
        {
            var phoneNumberString = phoneNumber.ToString();
            return phoneNumberString.Length == 10;
        }

        private bool BeUniquePhoneNumber(long phoneNumber)
        {
            var exists = _dbContext.Customers.Any(c => c.PhoneNumber == phoneNumber);
            return !exists;
        }
    }
}
