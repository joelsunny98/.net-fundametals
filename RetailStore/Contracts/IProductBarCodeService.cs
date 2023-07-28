using IronBarCode;

namespace RetailStore.Contracts;

/// <summary>
/// Service for generating barcodes for products.
/// </summary>
public interface IProductBarCodeService
{
    /// <summary>
    /// Generates Barcode for Product by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<GeneratedBarcode> GeneratedBarcode(int id);
}
