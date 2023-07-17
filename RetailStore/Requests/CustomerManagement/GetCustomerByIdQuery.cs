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
                _logger.LogWarning("Customer not found with ID: {CustomerId}", query.CustomerId);
                throw new KeyNotFoundException();
            }

            var result = new CustomerDto
            {
                CustomerName = customer.Name,
                PhoneNumber = (long)customer.PhoneNumber,
            };

            _logger.LogInformation("Retrieved customer with ID: {CustomerId}", query.CustomerId);

            return result;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to retrieve customer with ID: {CustomerId}", query.CustomerId);
            throw;
        }
    }

}

