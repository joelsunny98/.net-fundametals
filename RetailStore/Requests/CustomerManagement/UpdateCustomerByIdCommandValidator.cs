using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement
{
    /// <summary>
    /// Validator for Update Customer Command
    /// </summary>
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        private readonly RetailStoreDbContext _dbContext;
        /// <summary>
        /// Validator for defining specific rules for properties
        /// </summary>
        public UpdateCustomerCommandValidator(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(command => command.CustomerId)
                .GreaterThan(0).WithMessage(ValidationMessage.Invalid);

            RuleFor(command => command.CustomerName)
                .NotNull().NotEmpty().WithMessage(command => string.Format(ValidationMessage.Required, "Customer Name"))
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
