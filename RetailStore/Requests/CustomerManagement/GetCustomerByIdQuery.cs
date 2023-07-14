using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement;

public class GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public long? CustomerId { get; set; }
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
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == query.CustomerId);

        if (customer == null)
        {
            throw new KeyNotFoundException();
        }

        var result = new CustomerDto
        {
            CustomerName = customer.Name,
            PhoneNumber = (long)customer.PhoneNumber,
        };

        return result;
    }
}

