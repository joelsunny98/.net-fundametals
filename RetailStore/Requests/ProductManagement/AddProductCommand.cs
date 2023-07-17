using MediatR;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Command to add a new Product
    /// </summary>
    public class AddProductCommand : IRequest<int>
    {
        /// <summary>
        /// Gets and sets Data
        /// </summary>
        public ProductDto Data { get; set; }

    }

    /// <summary>
    /// Handler for the add product command
    /// </summary>
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Inject Dependency classes
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public AddProductCommandHandler(RetailStoreDbContext dbContext, ILogger<AddProductCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        /// <summary>
        /// Adds a new product to database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Product Id</returns>
        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Added Product to Databaase ");
            var product = new Product
            {
                Name = request.Data.ProductName,
                Price = request.Data.ProductPrice,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }


}
