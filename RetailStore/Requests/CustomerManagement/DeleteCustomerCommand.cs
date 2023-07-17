using MediatR;
using RetailStore.Model;
using RetailStore.Repository;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Command to Delete Customer by Id
/// </summary>
public class DeleteCustomerCommand : IRequest<Customer>
{
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
        try
        {
            var deletedCustomer = await _customerRepository.Delete(command.CustomerId);

            if (deletedCustomer == null)
            {
                throw new KeyNotFoundException();
            }

            _logger.LogInformation("Customer deleted: {CustomerId}", deletedCustomer.Id);

            return deletedCustomer;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to delete customer with ID: {CustomerId}", command.CustomerId);
            throw;
        }
    }
}