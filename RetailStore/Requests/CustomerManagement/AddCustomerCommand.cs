using MediatR;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Command to Add a new Customer
/// </summary>
public class AddCustomerCommand : CustomerDto, IRequest<int>
{
}

/// <summary>
/// Command Handler for Create Order Command
/// </summary>
public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, int>
{
    private readonly RetailStoreDbContext _dbContext;
    private readonly ILogger<AddCustomerCommandHandler> _logger;


    /// <summary>
    /// Injects RetailStoreDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    public AddCustomerCommandHandler(RetailStoreDbContext dbContext, ILogger<AddCustomerCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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

        _logger.LogInformation("New customer added: {CustomerId}", customer.Id);

        return customer.Id;
    }



}
