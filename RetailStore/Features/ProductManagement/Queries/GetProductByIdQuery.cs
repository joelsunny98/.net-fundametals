using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Features.ProductManagement.Queries
{
    public class GetProductByIdQuery: IRequest<List<ProductDto>>
    {
        public int Id { get; set; } 
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, List<ProductDto>>
    {
        private readonly RetailStoreDbContext _dbContext;

        /// <summary>
        /// Injects RetailDbContext class
        /// </summary>
        /// <param name="dbContext"></param>
        public GetProductByIdQueryHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

            return result;
        }


    }
}
