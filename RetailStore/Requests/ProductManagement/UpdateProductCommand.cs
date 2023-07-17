using MediatR;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Command to Update Product
    /// </summary>
    public class UpdateProductCommand : IRequest<int>
    {
        /// <summary>
        /// Gets and sets ProductId
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets and sets ProductData
        /// </summary>
        public ProductDto ProductData { get; set; }
    }

    /// <summary>
    /// Handler For Update Product Command
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;

        /// <summary>
        /// Injects RetailStoreDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public UpdateProductCommandHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Updates Product by Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return 0; // Or throw an exception to indicate the product was not found
            }

            product.Name = request.ProductData.ProductName;
            product.Price = request.ProductData.ProductPrice;
            product.UpdatedOn = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
