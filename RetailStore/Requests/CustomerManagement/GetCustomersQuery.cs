using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Query to get Cutomers
/// </summary>
public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
}

/// <summary>
/// Handler for Get Customer Query
/// </summary>
public class GetCustomerQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly RetailStoreDbContext _dbContext;
    private readonly ILogger<GetCustomerQueryHandler> _logger;

    /// <summary>
    /// Injects Dependencies
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetCustomerQueryHandler(RetailStoreDbContext dbContext, ILogger<GetCustomerQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Fetches all Customers from Database
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.Customers
                .Select(x => new CustomerDto
                {
                    CustomerName = x.Name,
                    PhoneNumber = (long)x.PhoneNumber
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation(LogMessage.GetAllItems, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessage.FailedToGetAllItems);
            throw;
        }
    }

}
