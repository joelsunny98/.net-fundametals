using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;
using System.Reflection.Metadata.Ecma335;

namespace RetailStore.Controllers;

[ApiController]
[Route("api")]
public class OrderDetailController: ControllerBase
{
    private readonly RetailStoreDbContext _dbContext;

    public OrderDetailController(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("order-details/best-seller")]
    public async Task<IActionResult> GetBestSeller()
    {
        var bestSeller = await _dbContext.OrderDetails.Include(t => t.Product).GroupBy(c => c.ProductId).OrderByDescending(g => g.Sum(q => q.Quantity)).Select(g => new
        {
            ProductId = g.Key,
            TotalQuantity = g.Sum(od => od.Quantity),
            ProductName = g.First().Product.Name
        }).FirstOrDefaultAsync();

        return Ok(bestSeller);
    }
}
