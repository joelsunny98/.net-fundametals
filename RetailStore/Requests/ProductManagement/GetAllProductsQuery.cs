using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Query to get all products
/// </summary>
public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
}

/// <summary>
/// Handler for Get all Product query
/// </summary>
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailDbContextClass
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetAllProductsQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetAllProductsQuery> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Fetches all products from the database
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of Products</returns>
    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products.Select(e => new ProductDto
        {
            ProductName = e.Name,
            ProductPrice = e.Price
        }).ToListAsync(cancellationToken);

        _logger.LogInformation(LogMessage.GetAllItems, products.Count);
        return products;
    }
}
