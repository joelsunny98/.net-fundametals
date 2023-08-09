using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Command to Update Product
/// </summary>
public class UpdateProductCommand : UpdateProductData, IRequest<int>
{
}

/// <summary>
/// Handler For Update Product Command
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailStoreDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public UpdateProductCommandHandler(IRetailStoreDbContext dbContext, ILogger<UpdateProductCommand> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Updates Product by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(request.Id);
        if (product == null)
        {
            _logger.LogError(LogMessage.SearchFail, request.Id);
            throw new KeyNotFoundException();
        }

        product.Name = request.ProductName;
        product.Price = request.ProductPrice;
        product.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LogMessage.UpdatedItem, request.Id);
        return product.Id;
    }
}