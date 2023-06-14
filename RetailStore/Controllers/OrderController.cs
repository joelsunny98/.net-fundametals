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
public class OrderController: ControllerBase
{
    private readonly IRepository<Order> customerRepository;
    private readonly RetailStoreDbContext _dbContext;
    public OrderController(IRepository<Order> _customerRepository, RetailStoreDbContext dbContext)
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
                    ProductId = d.Product.Id,
                    Quantity = d.Quantity
                }).ToList()
            }).ToListAsync();
        return Ok(orderDetails);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> AddOrders(OrderRequestDto order) 
    {
        var createdOrder = new Order
        {
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
        };
        _dbContext.Orders.Add(createdOrder);

        var details = order.Details.Select(d => new OrderDetail
        {
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            Order = createdOrder 
        }).ToList();

        _dbContext.OrderDetails.AddRange(details);

        await _dbContext.SaveChangesAsync();

        return Ok(createdOrder.Id);


    }

    [HttpDelete("orders")]
    public async Task<IActionResult> DeleteOrders(int id) 
    {
        var deletedOrder = await customerRepository.Delete(id);

        if (deletedOrder == null) 
        {
            return NotFound();
        }
        return Ok(deletedOrder);
    }

    [HttpPut("orders")]
    public async Task<IActionResult> UpdateOrder(Order order) 
    {
        var updatedOrder = await customerRepository.Update(order);
        return Ok(updatedOrder);
    }
}
