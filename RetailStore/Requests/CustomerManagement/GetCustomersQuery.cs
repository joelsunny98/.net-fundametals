using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Features.CustomerManagement;

public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
}

public class GetCustomerQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly RetailStoreDbContext _dbContext;

    public GetCustomerQueryHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Customers.Select(x => new CustomerDto
        {
            CustomerName = x.Name,
            PhoneNumber = (long)x.PhoneNumber
        }).ToListAsync();

        return result;
    }
}
