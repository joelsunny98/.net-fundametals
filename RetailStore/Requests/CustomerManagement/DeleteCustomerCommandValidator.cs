using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement
{
    /// <summary>
    /// Validator for Delete Customer Command
    /// </summary>
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        private readonly RetailStoreDbContext _dbContext;

        /// <summary>
        /// Validator to validate the Delete Customer Command
        /// </summary>
        public DeleteCustomerCommandValidator(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.CustomerId)
                .GreaterThan(0).WithMessage(ValidationMessage.CustomerIDGreaterThanZero)
                .MustAsync(BeExistingCustomerId).WithMessage(e => string.Format(ValidationMessage.CustomerIdDoesNotExist, e.CustomerId));
        }

        private async Task<bool> BeExistingCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var customerExists = await _dbContext.Customers.AnyAsync(c => c.Id == customerId, cancellationToken);
            return customerExists;
        }
    }
}
