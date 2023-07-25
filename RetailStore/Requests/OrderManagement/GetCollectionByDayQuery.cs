﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;

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
        private readonly IRetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Inject RetailDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public GetCollectionByDayQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetCollectionByDayQuery> logger)
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
            var totalCollection = await _dbContext.Orders.Where(e => e.CreatedOn.Date == DateTime.UtcNow.Date).SumAsync(e => e.TotalAmount, cancellationToken);

            _logger.LogInformation(LogMessage.DayCollection);
            return totalCollection.ConvertToCurrencyString();
        }
    }
}
