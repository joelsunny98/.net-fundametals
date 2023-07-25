using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Query to fetch Product by Id
    /// </summary>
    public class GetProductByIdQuery : IRequest<List<ProductDto>>
    {
        /// <summary>
        /// Gets and sets Id
        /// </summary>
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, List<ProductDto>>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Injects RetailDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public GetProductByIdQueryHandler(RetailStoreDbContext dbContext, ILogger<GetProductByIdQuery> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Fetches Product by Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Product</returns>
        public async Task<List<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Products
                .Where(e => e.Id == request.Id)
                .Select(e => new ProductDto
                {
                    ProductName = e.Name,
                    ProductPrice = e.Price,
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation(LogMessage.GetItemById, request.Id);
            return result;
        }
    }
}
