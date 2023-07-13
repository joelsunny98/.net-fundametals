using MediatR;
using RetailStore.Dtos;
using RetailStore.Persistence;

public class GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public int CustomerId { get; set; }
}

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly RetailStoreDbContext _dbContext;

    public GetCustomerByIdQueryHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FindAsync(query.CustomerId);

        if (customer == null)
        {
            return null;
        }

        var customerDto = new CustomerDto
        {
            CustomerName = customer.Name,
            PhoneNumber = customer.PhoneNumber,
        };

        return customerDto;
    }
}
