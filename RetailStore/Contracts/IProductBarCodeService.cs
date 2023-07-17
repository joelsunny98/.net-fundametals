using IronBarCode;

namespace RetailStore.Contracts;

public interface IProductBarCodeService
{
    public Task<GeneratedBarcode> GeneratedBarcode(int id);
}
