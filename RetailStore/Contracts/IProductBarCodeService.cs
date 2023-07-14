using IronBarCode;

namespace RetailStore.Contracts;

public interface IProductBarCodeService
{
    public Task<Image> GeneratedBarcode(int id);
}
