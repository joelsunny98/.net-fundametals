using Microsoft.AspNetCore.Mvc;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.OrderManagement;
using System.Net;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing Order of Retailstore
/// </summary>
[ApiController]
public class OrderController : BaseController
{
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
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddOrders([FromBody] CreateOrderCommand request)
    {
        var result = await Mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to delete a order by ID.
    /// </summary>
    /// <param name="id">order's Id to fetch order's data</param>
    [HttpDelete("orders/{id}")]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteOrders([FromRoute] int id)
    {
        var deletedOrder = await Mediator.Send(new DeleteOrderByIdCommand { Id = id });
        return Ok(deletedOrder);
    }

    /// <summary>
    /// Endpoint to fetch details of an order with given id.
    /// </summary>
    /// <param name="id">Order's Id to fetch order's data</param>
    /// <param name="orderRequestBody"></param>
    [HttpPut("orders/{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderRequestDto orderRequestBody)
    {
        var result = await Mediator.Send(new UpdateOrderByIdCommand
        {
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
    [ProducesResponseType(typeof(List<CustomerByOrderDto>), (int)HttpStatusCode.OK)]
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
    [ProducesResponseType(typeof(List<OrderDto>), (int)HttpStatusCode.OK)]
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
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCollectionByDay()
    {
        var result = await Mediator.Send(new GetCollectionByDayQuery());
        return Ok(result);
    }
}
