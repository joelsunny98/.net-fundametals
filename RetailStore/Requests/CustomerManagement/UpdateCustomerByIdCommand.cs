using FluentValidation;
using MediatR;
using RetailStore.Persistence;

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
    private readonly RetailStoreDbContext _dbContext;

    public UpdateCustomerCommandHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerCommandValidator();
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

        return customer.Id;
    }
}
