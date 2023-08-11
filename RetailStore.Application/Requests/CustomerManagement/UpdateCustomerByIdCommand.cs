using FluentValidation;
using MediatR;
using RetailStore.Constants;
using RetailStore.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCustomerCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The retail store database context.</param>
    public UpdateCustomerCommandHandler(IRetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Updates the customer with the provided Id.
    /// </summary>
    /// <param name="command">The update customer command containing the new customer data.</param>
    /// <param name="cancellationToken">The cancellation token to cdefancel the async operation.</param>
    /// <returns>Returns the Id of the updated customer.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the customer with the specified Id is not found.</exception>
    public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {

        var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == command.CustomerId);

        customer.Name = command.CustomerName;
        customer.PhoneNumber = command.PhoneNumber;
        customer.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        //_logger.LogInformation(LogMessage.UpdatedItem, customer.Id);

        return customer.Id;
    }
}
