using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
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
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;


    public GetCustomerByIdQueryHandler(RetailStoreDbContext dbContext, ILogger<GetCustomerByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<CustomerDto> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        try
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

            _logger.LogInformation(LogMessage.GetItemById, query.CustomerId);

            return result;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, LogMessage.SearchFail, query.CustomerId);
            throw;
        }
    }

}

