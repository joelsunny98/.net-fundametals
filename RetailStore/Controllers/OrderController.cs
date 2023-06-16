using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Extensions;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing Order of Retailstore
/// </summary>
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

    /// <summary>
    /// Endpoint to fetch details of orders of retail store.
    /// </summary>
    /// <returns>It returns order details</returns>
    [HttpGet("orders")]
    [ProducesResponseType(typeof(List<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrders() 
    {
        var orderDetails = await _dbContext.Orders.Include(e => e.Details).ThenInclude(e => e.Product).Include(e => e.Customer)
            .Select(e => new OrderDto() { 
                CustomerName = e.Customer.Name,
                Amount = (e.TotalAmount + e.Discount).AddDecimalPoints(),
                Discount = e.Discount.AddDecimalPoints(),
                TotalAmount = e.TotalAmount.AddDecimalPoints(),
                Details = e.Details.Select(d => new OrderDetailDto() 
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity
                }).ToList()
            }).ToListAsync();
        return Ok(orderDetails);
    }

    /// <summary>
    /// Adding data of order to the database
    /// </summary>
    /// <returns>
    /// Id of order inserted to the record
    /// </returns> 
    [HttpPost("orders")]
    public async Task<IActionResult> AddOrders(OrderRequestDto order) 
    {
        var createdOrder = new Order
        {
            CustomerId = order.CustomerId,
            TotalAmount = 0,
            Discount = 0,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        };
        _dbContext.Orders.Add(createdOrder);

         var details = order.Details.Select(d =>
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == d.ProductId);
            var orderDetail = new OrderDetail
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                Order = createdOrder
            };

            if (product != null)
            {
                var Amount = createdOrder.TotalAmount.TotalValue(product.Price, d.Quantity);
                createdOrder.TotalAmount = Amount.DiscountedAmount; 
                createdOrder.Discount = Amount.DiscountValue;
            }
            return orderDetail;
        }).ToList();
        _dbContext.OrderDetails.AddRange(details);
        await _dbContext.SaveChangesAsync();
        return Ok(createdOrder.Id);
    }

    /// <summary>
    /// Endpoint to delete a order by ID.
    /// </summary>
    /// <param name="id">order's Id to fetch order's data</param>
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

    /// <summary>
    /// Endpoint to fetch details of an order with given id.
    /// </summary>
    /// <param name="id">Order's Id to fetch order's data</param>
    [HttpPut("orders/{id}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateOrder(int id, OrderRequestDto orderRequestBody)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = orderRequestBody.CustomerId,
            UpdatedOn = DateTime.UtcNow,
        };

        var updatedOrder = await customerRepository.Update(order);
        return Ok(updatedOrder);
    }

    /// <summary>
    /// Endpoint to fetch a sum of orders made by each customer
    /// </summary>
    /// <returns>
    /// Object
    /// </returns>
    [HttpGet("orders/customer")]
    public async Task<IActionResult> GetOrderByCustomer() 
    {
        var result = await _dbContext.Orders.Include(t => t.Customer).GroupBy(c => c.CustomerId).Select(g => new
        {
            CustomerName = g.FirstOrDefault().Customer.Name,
            TotalOrders = g.Count()
        }).ToListAsync();

        return Ok(result);
    }

    /// <summary>
    /// Endpoint to fetch a list of orders made today
    /// </summary>
    /// <returns></returns>
    [HttpGet("orders/today")]
    public async Task<IActionResult> GetOrderByDay() 
    {
        var result = await _dbContext.Orders.Include(e => e.Details).Where(e => e.CreatedOn.Date == DateTime.UtcNow.Date).Select(e => new OrderDto 
        {
            CustomerName = e.Customer.Name,
            Amount = (e.TotalAmount + e.Discount).AddDecimalPoints(),
            Discount = e.Discount.AddDecimalPoints(),
            TotalAmount = e.TotalAmount.AddDecimalPoints(),
            Details = e.Details.Select(d => new OrderDetailDto()
            {
                ProductName = d.Product.Name,
                Quantity = d.Quantity
            }).ToList()
        }).ToListAsync();

        return Ok(result);
    }

}
