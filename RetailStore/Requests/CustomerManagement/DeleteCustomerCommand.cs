using MediatR;
using RetailStore.Constants;
using RetailStore.Model;
using RetailStore.Repository;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Command to Delete Customer by Id
/// </summary>
public class DeleteCustomerCommand : IRequest<Customer>
{
    /// <summary>
    /// Gets and sets CustomerId
    /// </summary>
    public int CustomerId { get; set; }
}

    /// <summary>
    /// Handler for Delete Customer by Id command
    /// </summary>
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Customer>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;


    /// <summary>
    /// Injects IRepository class
    /// </summary>
    /// <param name="customerRepository"></param>
    /// <param name="logger"></param>
    public DeleteCustomerCommandHandler(IRepository<Customer> customerRepository, ILogger<DeleteCustomerCommandHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

        /// <summary>
        /// Deletes Customer by Id
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Customer</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Customer> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            // No need for the if condition here, validation will be done using the validator.
            var deletedCustomer = await _customerRepository.Delete(command.CustomerId);

            _logger.LogInformation(LogMessage.DeleteItem, deletedCustomer.Id);

            return deletedCustomer;
        }
    }

