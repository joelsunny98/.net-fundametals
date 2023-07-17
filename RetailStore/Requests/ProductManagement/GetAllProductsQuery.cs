using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Query to get all products
    /// </summary>
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
    }

    /// <summary>
    /// Handler for Get all Product query
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Injects RetailDbContextClass
        /// </summary>
        /// <param name="dbContext"></param>
        public GetAllProductsQueryHandler(RetailStoreDbContext dbContext, ILogger<GetAllProductsQuery> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all products from the database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of Products</returns>
        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products.Select(e => new ProductDto 
            {
                ProductName = e.Name,
                ProductPrice = e.Price
            }).ToListAsync();

            _logger.LogInformation("Retrieved {ProductCount} Products", products.Count);
            return products;
        }
    }


}
