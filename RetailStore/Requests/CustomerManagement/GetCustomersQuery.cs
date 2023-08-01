using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Query to get all customers
/// </summary>
public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
}

/// <summary>
/// Handler for Get all Product query
/// </summary>
public class GetCustomerQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger<GetCustomerQueryHandler> _logger;

    /// <summary>
    /// Injects RetailDbContextClass
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetCustomerQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetCustomerQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Fetches all customers from the database
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of Customers</returns>
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
