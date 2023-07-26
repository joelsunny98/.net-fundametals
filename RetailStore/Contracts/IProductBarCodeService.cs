using IronBarCode;

namespace RetailStore.Contracts;

/// <summary>
/// Interface for Product Barcode Service
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
