using IronBarCode;
using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Persistence;

namespace RetailStore.Services;

public class ProductBarCodeService : IProductBarCodeService
{
    private readonly RetailStoreDbContext _dbContext;
    public ProductBarCodeService(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GeneratedBarcode> GeneratedBarcode(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException(nameof(product));
        }

        var barcode = BarcodeWriter.CreateBarcode(product.Name.ToString(), BarcodeWriterEncoding.Code128);

        

        Image barcodeImage = barcode.Image;


        return barcode;
    }
}
