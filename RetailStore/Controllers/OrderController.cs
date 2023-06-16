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
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly RetailStoreDbContext _dbContext;
    public OrderController(IRepository<Order> orderRepository, RetailStoreDbContext dbContext, IRepository<Product> productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
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
            .Select(e => new OrderDto()
            {
                Id = e.Id,
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
            var product = _productRepository.GetById(d.ProductId).Result;
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
        var deletedOrder = await _orderRepository.Delete(id);

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
    [HttpPut("orders")]
    public async Task<IActionResult> UpdateOrder(Order order)
    {
        var updatedOrder = await _orderRepository.Update(order);
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
            Id = e.Id,
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

    /// <summary>
    /// Endpoint to fetch a list of orders made today
    /// </summary>
    /// <returns></returns>
    [HttpGet("orders/collection")]
    public async Task<IActionResult> GetCollectionByDay()
    {
        var totalCollection = await _dbContext.Orders
        .Where(e => e.CreatedOn.Date == DateTime.UtcNow.Date)
        .SumAsync(e => e.TotalAmount);

        return Ok(totalCollection.AddDecimalPoints());
    }
}
