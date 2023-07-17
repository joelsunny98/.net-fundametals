using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;

namespace RetailStore.Requests.OrderManagement
{
    /// <summary>
    /// Query to Get Collection by day
    /// </summary>
    public class GetCollectionByDayQuery : IRequest<string>
    {
    }

    /// <summary>
    /// Handler for the Get collection by day Query
    /// </summary>
    public class GetCollectionByDayQueryHandler : IRequestHandler<GetCollectionByDayQuery, string>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Inject RetailDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public GetCollectionByDayQueryHandler(RetailStoreDbContext dbContext, ILogger<GetCollectionByDayQuery> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

        }

        /// <summary>
        /// Fetches the Total Collection of the day
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Total Collection</returns>
        public async Task<string> Handle(GetCollectionByDayQuery request, CancellationToken cancellationToken)
        {
            var totalCollection = await _dbContext.Orders.Where(e => e.CreatedOn.Date == DateTime.UtcNow.Date).SumAsync(e => e.TotalAmount);

            _logger.LogInformation("Retreived Total Collection for the day");
            return totalCollection.ConvertToCurrencyString();
        }
    }
}
