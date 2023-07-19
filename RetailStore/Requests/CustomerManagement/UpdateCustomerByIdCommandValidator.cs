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
                .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
                .MaximumLength(25).WithMessage(ValidationMessage.Length);

            RuleFor(command => command.PhoneNumber)
                  .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
                  .Must(BeValidPhoneNumber).WithMessage(ValidationMessage.PhoneNumberLength)
                  .Must(BeUniquePhoneNumber).WithMessage(ValidationMessage.UniquePhoneNumber);
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
