using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Net;

namespace RetailStore.Controllers;

[ApiController]
[Route("api")]
public class OrdeController: ControllerBase
{
    private readonly IRepository<Customer> customerRepository;
    private readonly RetailStoreDbContext _dbContext;
    public OrdeController(IRepository<Customer> _customerRepository, RetailStoreDbContext dbContext)
    {
        customerRepository = _customerRepository;
        _dbContext = dbContext;
    }

    [HttpGet("orders")]
    [ProducesResponseType(typeof(List<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrders() 
    {
        var orderDetails = await _dbContext.Orders.Include(e => e.Details).ThenInclude(e => e.Product).Include(e => e.Customer)
            .Select(e => new OrderDto() { 
                Id = e.Id,
                CustomerName = e.Customer.Name,
                TotalAmount = e.TotalAmount,
                Details = e.Details.Select(d => new OrderDetailDto() 
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity
                }).ToList()
            }).ToListAsync();
        return Ok(orderDetails);
    }
}
