using MediatR;
using RetailStore.Constants;
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
        private readonly ILogger _logger;

        /// <summary>
        /// Injects RetailStoreDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public DeleteProductCommandHandler(RetailStoreDbContext dbContext, ILogger<DeleteProductCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
                _logger.LogError(LogMessage.SearchFail, request.ProductId);
                throw new KeyNotFoundException();
            }

            _dbContext.Products.Remove(deletedProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(LogMessage.DeleteItem, request.ProductId);
            return deletedProduct.Id;
        }
    }
}
