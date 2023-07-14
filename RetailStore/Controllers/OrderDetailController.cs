using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Extentions;
using RetailStore.Persistence;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for Best seller of Retailstore
/// </summary>
[ApiController]
[Route("api")]
public class OrderDetailController : ControllerBase
{
    private readonly RetailStoreDbContext _dbContext;

    public OrderDetailController(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Endpoint to fetch details of a best selling product.
    /// </summary>
    /// <returns>It returns best selling product details</returns>
    [HttpGet("order-details/best-seller")]
    public async Task<IActionResult> GetBestSeller()
    {
        var bestSeller = await _dbContext.OrderDetails.Include(t => t.Product).GroupBy(c => c.ProductId).OrderByDescending(g => g.Sum(q => q.Quantity)).Select(g => new BestSellerDto
        {
            ProductId = g.Key,
            Quantity = g.Sum(od => od.Quantity),
            ProductName = g.First().Product.Name,
            Price = g.First().Product.Price,
            TotalRevenue = g.First().Product.Price.TotalRevenue(g.Sum(od => od.Quantity))
        }).FirstOrDefaultAsync();

        return Ok(bestSeller);
    }
}
