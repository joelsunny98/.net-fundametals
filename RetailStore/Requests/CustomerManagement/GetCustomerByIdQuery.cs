using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Query to fetch customer by Id
/// </summary>
public class GetCustomerByIdQuery : IRequest<CustomerDto>
{

    /// <summary>
    /// Gets and sets Id
    /// </summary>
    public long? CustomerId { get; set; }
}

/// <summary>
/// Handler for Get Customer By Id command
/// </summary>
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

    /// <summary>
    /// Injects RetailDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetCustomerByIdQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetCustomerByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Fetches customer by Id
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Customer</returns>
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

