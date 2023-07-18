﻿using IronBarCode;
using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Persistence;

namespace RetailStore.Services;

public class ProductBarCodeService : IProductBarCodeService
{
    private readonly RetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    public ProductBarCodeService(RetailStoreDbContext dbContext, ILogger<ProductBarCodeService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<GeneratedBarcode> GeneratedBarcode(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            _logger.LogError("Prouct with {ProductId} not found", product.Id);
            throw new KeyNotFoundException(nameof(product));
        }

        var barcode = BarcodeWriter.CreateBarcode(product.Name.ToString(), BarcodeWriterEncoding.Code128);

        _logger.LogInformation("Barcode for Product {ProductId} generated", product.Id);
        return barcode;
    }
}
