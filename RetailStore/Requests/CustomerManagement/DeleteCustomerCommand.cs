using MediatR;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;
using RetailStore.Requests.OrderManagement;

namespace RetailStore.Features.CustomerManagement;

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

    /// <summary>
    /// Injects IRepository class
    /// </summary>
    /// <param name="customerRepository"></param>
    public DeleteCustomerCommandHandler(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
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
        var deletedCustomer = await _customerRepository.Delete(command.CustomerId);

        if (deletedCustomer == null)
        {
            throw new KeyNotFoundException();
        }

        return deletedCustomer;

    }
}