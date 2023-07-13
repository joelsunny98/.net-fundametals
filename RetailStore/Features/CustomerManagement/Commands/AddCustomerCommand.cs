using MediatR;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Command to Add a new Customer
/// </summary>
public class AddCustomerCommand : IRequest<int>
{
    /// <summary>
    /// Gets and sets Data
    /// </summary>
    public CustomerDto Data { get; set; }
}

/// <summary>
/// Command Handler for Create Order Command
/// </summary>
public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, int>
{
    private readonly RetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailStoreDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    public AddCustomerCommandHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a new customer to the database
    /// </summary>
    /// <param name="command">The command containing customer data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created customer</returns>
    public async Task<int> Handle(AddCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = command.CustomerName,
            PhoneNumber = command.PhoneNumber,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }

}
