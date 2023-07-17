using MediatR;
using RetailStore.Contracts;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Gets Product Barcode
/// </summary>
public class GetProductBarcodeQuery : IRequest<string>
{
    /// <summary>
    /// Gets and sets Product ID
    /// </summary>
    public int ProductId { get; set; }
}

/// <summary>
/// Handler for the Get Product Barcode Query
/// </summary>
public class GetProductBarcodeQueryHandler : IRequestHandler<GetProductBarcodeQuery, string>
{
    private readonly IProductBarCodeService _barcodeServcie;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailStoreDbContext class
    /// </summary>
    /// <param name="barCodeService"></param>
    public GetProductBarcodeQueryHandler(IProductBarCodeService barCodeService, ILogger<GetProductBarcodeQuery> logger)
    {
        _barcodeServcie = barCodeService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a BardCode for a product by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(GetProductBarcodeQuery request, CancellationToken cancellationToken)
    {
        var barcode = await _barcodeServcie.GeneratedBarcode(request.ProductId);

        var result = barcode.Value;

        _logger.LogInformation("Generated Product Bardcode");
        return result;
    }
}
