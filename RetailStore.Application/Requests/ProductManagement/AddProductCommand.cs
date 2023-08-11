using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Command to add a new Product
/// </summary>
public class AddProductCommand : ProductDto, IRequest<int>
{
}

/// <summary>
/// Handler for the add product command
/// </summary>
public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
{
    private readonly IRetailStoreDbContext _dbContext;

    /// <summary>
    /// Inject Dependency classes
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public AddProductCommandHandler(IRetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a new product to database
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Product Id</returns>
    public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {

        var product = new Product
        {
            Name = request.ProductName,
            Price = request.ProductPrice,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}