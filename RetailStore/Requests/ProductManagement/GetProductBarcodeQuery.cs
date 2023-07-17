using MediatR;
using RetailStore.Contracts;

namespace RetailStore.Requests.ProductManagement;

public class GetProductBarcodeQuery : IRequest<string>
{
    public int ProductId { get; set; }
}

public class GetProductBarcodeQueryHandler : IRequestHandler<GetProductBarcodeQuery, string>
{
    private readonly IProductBarCodeService _barcodeServcie;

    public GetProductBarcodeQueryHandler(IProductBarCodeService barCodeService)
    {
        _barcodeServcie = barCodeService;
    }

    public async Task<string> Handle(GetProductBarcodeQuery request, CancellationToken cancellationToken)
    {
        var barcode = await _barcodeServcie.GeneratedBarcode(request.ProductId);

        var result = barcode.Value;

        return result;
    }
}
