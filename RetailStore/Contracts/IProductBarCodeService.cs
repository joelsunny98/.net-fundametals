using IronBarCode;

namespace RetailStore.Contracts;

public interface IProductBarCodeService
{
    /// <summary>
    /// Generates Barcode for Product by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<GeneratedBarcode> GeneratedBarcode(int id);
}
