using IronBarCode;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Services;

/// <summary>
/// Service to Product Barcode
/// </summary>
public class ProductBarCodeService : IProductBarCodeService
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects dependencies
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public ProductBarCodeService(IRetailStoreDbContext dbContext, ILogger<ProductBarCodeService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Method to Generate Bardcode for Product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<GeneratedBarcode> GeneratedBarcode(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            _logger.LogError(LogMessage.SearchFail, id);
            throw new KeyNotFoundException(nameof(product));
        }

        var barcode = BarcodeWriter.CreateBarcode(product.Name.ToString(), BarcodeWriterEncoding.Code128);

        _logger.LogInformation(LogMessage.BarcodeGenerated, product.Id);
        return barcode;
    }
}
