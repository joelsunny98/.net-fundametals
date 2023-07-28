using FluentValidation;
using MediatR;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Requests.CustomerManagement;

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
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    /// <summary>
    /// Injects Dependencies
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public UpdateCustomerCommandHandler(IRetailStoreDbContext dbContext, ILogger<UpdateCustomerCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Updates Customer with Id
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerCommandValidator(_dbContext);
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

        if (customer == null)
        {
            throw new KeyNotFoundException();
        }

        customer.Name = command.CustomerName;
        customer.PhoneNumber = command.PhoneNumber;
        customer.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LogMessage.UpdatedItem, customer.Id);

        return customer.Id;
    }
}
