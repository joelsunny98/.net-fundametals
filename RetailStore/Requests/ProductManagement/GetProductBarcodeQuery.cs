using MediatR;
using RetailStore.Contracts;

namespace RetailStore.Requests.ProductManagement;

public class GetProductBarcodeQuery : IRequest<Image>
{
    public int ProductId { get; set; }
}

public class GetProductBarcodeQueryHandler : IRequestHandler<GetProductBarcodeQuery, Image>
{
    private readonly IProductBarCodeService _barcodeServcie;

    public GetProductBarcodeQueryHandler(IProductBarCodeService barCodeService)
    {
        _barcodeServcie = barCodeService;
    }

    public async Task<Image> Handle(GetProductBarcodeQuery request, CancellationToken cancellationToken)
    {
        var barcode = await _barcodeServcie.GeneratedBarcode(request.ProductId);

        return barcode;
    }
}
