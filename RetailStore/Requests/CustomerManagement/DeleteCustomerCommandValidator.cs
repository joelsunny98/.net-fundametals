using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Persistence;
using RetailStore.Constants;
using RetailStore.Model;
using RetailStore.Repository;
using System.Threading.Tasks;

namespace RetailStore.Requests.CustomerManagement
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        private readonly IRepository<Customer> _customerRepository;

        public DeleteCustomerCommandValidator(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(command => command.CustomerId)
                .GreaterThan(0).WithMessage(ValidationMessage.CustomerIDGreaterThanZero)
                .MustAsync(BeExistingCustomerId).WithMessage(e => string.Format(ValidationMessage.CustomerIdDoesNotExist, e.CustomerId));
                .GreaterThan(0).WithMessage((command => string.Format(ValidationMessage.Valid, "CustomerId")))
                .MustAsync(NotExists).WithMessage(ValidationMessage.NotExist);
        }

        private async Task<bool> NotExists(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(customerId);
            return customer == null;
        }

    }
}
