using MediatR;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Delete a product by Id
    /// </summary>
    public class DeleteProductCommand : IRequest<int>
    {
        /// <summary>
        /// Gets and sets ProductId
        /// </summary>
        public int ProductId { get; set; }
    }

    /// <summary>
    /// Handler for the Delete Product Command
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;

        /// <summary>
        /// Injects RetailStoreDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public DeleteProductCommandHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Deletes a prodcut by Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deletedProduct = await _dbContext.Products.FindAsync(request.ProductId);
            if (deletedProduct == null)
            {
                return 0; // Or throw an exception to indicate the product was not found
            }

            _dbContext.Products.Remove(deletedProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return deletedProduct.Id;
        }
    }
}
