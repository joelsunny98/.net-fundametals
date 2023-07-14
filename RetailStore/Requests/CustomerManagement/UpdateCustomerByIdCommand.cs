using MediatR;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement;
public class UpdateCustomerCommand : IRequest<int>
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public long PhoneNumber { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
{
    private readonly RetailStoreDbContext _dbContext;

    public UpdateCustomerCommandHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

        if (customer == null)
        {
            return 0;
        }

        customer.Name = command.CustomerName;
        customer.PhoneNumber = command.PhoneNumber;
        customer.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
