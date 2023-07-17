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

        /// <summary>
        /// Injects RetailDbContextClass
        /// </summary>
        /// <param name="dbContext"></param>
        public GetAllProductsQueryHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Fetches all products from the database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of Products</returns>
        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products.ToListAsync(cancellationToken);
            var productsResponse = products.Select(e => new ProductDto
            {
                ProductName = e.Name,
                ProductPrice = e.Price
            }).ToList();

            return productsResponse;
        }
    }


}
