using MediatR;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Command to Update Product
    /// </summary>
    public class UpdateProductCommand :UpdateProductData ,IRequest<int>
    {
    }

    /// <summary>
    /// Handler For Update Product Command
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Injects RetailStoreDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public UpdateProductCommandHandler(RetailStoreDbContext dbContext, ILogger<UpdateProductCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Updates Product by Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(request.Id);
            if (product == null)
            {
                return 0; // Or throw an exception to indicate the product was not found
            }

            product.Name = request.ProductName;
            product.Price = request.ProductPrice;
            product.UpdatedOn = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Update Product with Id: {Product Id}", request.Id);
            return product.Id;
        }
    }
}
