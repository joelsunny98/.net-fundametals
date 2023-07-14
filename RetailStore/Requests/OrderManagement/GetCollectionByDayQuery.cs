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

        /// <summary>
        /// Inject RetailDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public GetCollectionByDayQueryHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
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

            return totalCollection.ConvertToCurrencyString();
        }
    }
}
