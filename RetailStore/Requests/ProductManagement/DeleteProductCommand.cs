using MediatR;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    public class DeleteProductCommand : IRequest<int>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly RetailStoreDbContext _dbContext;

        public DeleteProductCommandHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
