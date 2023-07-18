using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Extentions;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Requests.OrderDetailManagement;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for Best seller of Retailstore
/// </summary>
[ApiController]
[Route("api")]
public class OrderDetailController : BaseController
{
    /// <summary>
    /// Endpoint to fetch details of a best selling product.
    /// </summary>
    /// <returns>It returns best selling product details</returns>
    [HttpGet("order-details/best-seller")]
    public async Task<IActionResult> GetBestSeller()
    {
        var result = await Mediator.Send(new GetBestSellerQuery());
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to fetch the OrderSize by order Id
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("order-details/{orderId}/order-size")]
    public async Task<IActionResult> GetOrderSize([FromRoute]int orderId) 
    {
        var result = await Mediator.Send(new GetOrderSizeQuery { OrderId = orderId });
        return Ok(result);
    }
}
