using MediatR;
using RetailStore.Constants;
using RetailStore.Model;
using RetailStore.Repository;
using Twilio.TwiML.Messaging;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Command to Delete Customer by Id
/// </summary>
public class DeleteCustomerCommand : IRequest<string>
{
    /// <summary>
    /// Gets and sets Id
    /// </summary>
    public int CustomerId { get; set; }
}

/// <summary>
/// Handler for Delete Customer by Id command
/// </summary>
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, string>
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly ILogger<DeleteCustomerCommandHandler> _logger;

    /// <summary>
    /// Injects IRepository class
    /// </summary>
    /// <param name="customerRepository"></param>
    /// <param name="logger">The logger instance.</param>
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
    /// <returns>Response message</returns>
    /// <exception cref="ValidationException"></exception>
    public async Task<string> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetById(command.CustomerId);

        if (customer == null)
        {
            return string.Format(ValidationMessage.CustomerDoesNotExist, command.CustomerId);
        }

        var deletedCustomer = await _customerRepository.Delete(command.CustomerId);

        _logger.LogInformation(LogMessage.DeleteItem, deletedCustomer.Id);
        return string.Format(ValidationMessage.CustomerDeletedSuccessfully, command.CustomerId);
    }
}