using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Features.ProductManagement.Queries
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly RetailStoreDbContext _dbContext;

        public GetAllProductsQueryHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
