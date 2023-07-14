using MediatR;
using RetailStore.Persistence;

namespace RetailStore.Features.CustomerManagement;

/// <summary>
/// Update Command for Customer
/// </summary>
public class UpdateCustomerCommand : IRequest<int>
{
    /// <summary>
    /// Gets and sets Id
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets and sets Customer name
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets and sets PhoneNumber
    /// </summary>
    public long PhoneNumber { get; set; }
}

/// <summary>
/// Handler for Update Customer Command
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
{
    private readonly RetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailDbContext Class
    /// </summary>
    /// <param name="dbContext"></param>
    public UpdateCustomerCommandHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Updates Customer by Id
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Customer id</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

        if (customer == null)
        {
            throw new KeyNotFoundException();
        }

        customer.Name = command.CustomerName;
        customer.PhoneNumber = command.PhoneNumber;
        customer.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
