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

    public async Task<Image> GeneratedBarcode(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException(nameof(product));
        }

        var barcode = BarcodeWriter.CreateBarcode(product.ToString(), BarcodeWriterEncoding.Code128);

        barcode.SaveAsImage("barcode.png");

        Image barcodeImage = barcode.Image;


        return barcodeImage;
    }
}
