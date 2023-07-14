using MediatR;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;

namespace RetailStore.Features.ProductManagement.Commands
{
    public class AddProductCommand : IRequest<int>
    {
        /// <summary>
        /// Gets and sets Data
        /// </summary>
        public ProductDto Data { get; set; }
        
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;

        public AddProductCommandHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
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
