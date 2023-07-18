using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.CustomerManagement;

public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
}

public class GetCustomerQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly RetailStoreDbContext _dbContext;
    private readonly ILogger<GetCustomerQueryHandler> _logger;


    public GetCustomerQueryHandler(RetailStoreDbContext dbContext, ILogger<GetCustomerQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


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
                .ToListAsync();

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
