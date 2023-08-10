using FluentValidation;
using MediatR;
using RetailStore.Constants;
using RetailStore.Contracts;
using Microsoft.Extensions.Logging;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Update Command for Customer
/// </summary>
public class UpdateCustomerCommand : IRequest<int>
{
    /// <summary>
    /// Gets or sets the Id of the customer.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public long PhoneNumber { get; set; }
}

/// <summary>
/// Handler for Update Customer Command
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCustomerCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The retail store database context.</param>
    /// <param name="logger">The logger instance used for logging.</param>
    public UpdateCustomerCommandHandler(IRetailStoreDbContext dbContext, ILogger<UpdateCustomerCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Updates the customer with the provided Id.
    /// </summary>
    /// <param name="command">The update customer command containing the new customer data.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the async operation.</param>
    /// <returns>Returns the Id of the updated customer.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the customer with the specified Id is not found.</exception>
    public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {

        var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

        customer.Name = command.CustomerName;
        customer.PhoneNumber = command.PhoneNumber;
        customer.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LogMessage.UpdatedItem, customer.Id);

        return customer.Id;
    }
}
