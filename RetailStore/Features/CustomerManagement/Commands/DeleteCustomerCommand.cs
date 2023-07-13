using MediatR;
using RetailStore.Persistence;

public class DeleteCustomerCommand : IRequest<int>
{
    public int CustomerId { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, int>
{
    private readonly RetailStoreDbContext _dbContext;

    public DeleteCustomerCommandHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

        if (customer == null)
        {
            return default;
        }

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
