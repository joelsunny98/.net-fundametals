using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Extensions;
using RetailStore.Features.OrderManagement.Commands;
using RetailStore.Features.OrderManagement.Queries;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing Order of Retailstore
/// </summary>
[ApiController]
public class OrderController : BaseController
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
        var result = await Mediator.Send(new GetOrdersQuery());
        return Ok(result);
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
        var result = await Mediator.Send(new CreateOrderCommand { Data = order });
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to delete a order by ID.
    /// </summary>
    /// <param name="id">order's Id to fetch order's data</param>
    [HttpDelete("orders")]
    public async Task<IActionResult> DeleteOrders(int id)
    {
        var deletedOrder = await Mediator.Send(new DeleteOrderByIdCommand { Id = id });
        return Ok(deletedOrder);
    }

    /// <summary>
    /// Endpoint to fetch details of an order with given id.
    /// </summary>
    /// <param name="id">Order's Id to fetch order's data</param>
    [HttpPut("orders/{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateOrder(int id, OrderRequestDto orderRequestBody)
    {
        var result = await Mediator.Send(new UpdateOrderByIdCommand { 
            Id = id, 
            OrderRequest = orderRequestBody
        });
        return Ok(result);
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
        var result = await Mediator.Send(new GetOrderByCustomerQuery());
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to fetch a list of orders made today
    /// </summary>
    /// <returns></returns>
    [HttpGet("orders/today")]
    public async Task<IActionResult> GetOrderByDay()
    {
        var result = await Mediator.Send(new GetOrderByDayQuery());
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to fetch a list of orders made today
    /// </summary>
    /// <returns></returns>
    [HttpGet("orders/collection")]
    public async Task<IActionResult> GetCollectionByDay()
    {
        var result = await Mediator.Send(new GetCollectionByDayQuery());
        return Ok(result);
    }
}
