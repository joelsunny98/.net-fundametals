using Microsoft.AspNetCore.Mvc;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing Customer's of Retailstore
/// </summary>
[ApiController]
[Route("api")]
public class CustomerController : BaseController
{
    /// <summary>
    /// Endpoint to fetch details of an customer.
    /// </summary>
    /// <returns>It returns customer details</returns>
    [HttpGet("customers")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await Mediator.Send(new GetCustomersQuery());
        return Ok(result);
    }

    /// <summary>
    /// Adding data of customer to the database
    /// </summary>
    /// <returns>Id of customer inserted to the record</returns> 
    [HttpPost("customers")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCustomers(AddCustomerCommand request)
    {
        var result = await Mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a customer by ID.
    /// </summary>
    /// <param name="id">ID of the customer to be deleted</param>
    [HttpDelete("customers/{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
    {
        var deleteCustomer = await Mediator.Send(new DeleteCustomerCommand { CustomerId = id });
        return Ok(deleteCustomer);
    }

    /// <summary>
    /// Endpoint to fetch details of an customer with given id.
    /// </summary>
    /// <param name="id">Customers's Id to fetch customer's data</param>
    [HttpGet("customers/{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetCustomerByIdQuery
        {
            CustomerId = id
        });
        return Ok(result);
    }

    /// <summary>
    /// Updates a customer with the given ID.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="customerRequestBody">The updated customer information.</param>
    /// <returns>The updated customer.</returns>
    [HttpPut("customers/{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] CustomerDto customerRequestBody)
    {
        var result = await Mediator.Send(new UpdateCustomerCommand
        {
            CustomerId = id,
            CustomerName = customerRequestBody.CustomerName,
            PhoneNumber = customerRequestBody.PhoneNumber
        });
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to fetch details of a premium customer.
    /// </summary>
    /// <returns>It returns best purchasing customer details</returns>
    [HttpGet("premium")]
    public async Task<IActionResult> GetPremiumCustomers()
    {
        var premiumCustomers = await Mediator.Send(new GetPremiumCustomersQuery());
        return Ok(premiumCustomers);
    }
}
