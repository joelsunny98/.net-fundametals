using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Model;
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
                .GreaterThan(0).WithMessage("CustomerId must be greater than 0.")
                .MustAsync(BeExistingCustomerId).WithMessage("Customer with the given ID does not exist.");
        }

        private async Task<bool> BeExistingCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var customerExists = await _dbContext.Customers.AnyAsync(c => c.Id == customerId, cancellationToken);
            return customerExists;
        }
    }
}
