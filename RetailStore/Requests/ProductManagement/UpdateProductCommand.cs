using MediatR;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public ProductDto ProductData { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;

        public UpdateProductCommandHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
