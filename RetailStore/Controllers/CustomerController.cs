using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Features.CustomerManagement;

using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Formats.Asn1;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing Customer's of Retailstore
/// </summary>
[ApiController]
[Route("api")]
public class CustomerController : BaseController
{
    private readonly IRepository<Customer> _customerRepository;
    public CustomerController(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

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
    /// <returns>
    /// Id of customer inserted to the record
    /// </returns> 
    [HttpPost("customers")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCustomers(CustomerDto customer)
    {
        var result = await Mediator.Send(new AddCustomerCommand { Data = customer });
        return Ok(result);
    }


    /// <summary>
    /// Delete a customer by ID.
    /// </summary>
    /// <param name="id">ID of the customer to be deleted</param>
    [HttpDelete("customers/{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var result = await Mediator.Send(new DeleteCustomerCommand { CustomerId = id });

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Endpoint to fetch details of an customer with given id.
    /// </summary>
    /// <param name="id">Customers's Id to fetch customer's data</param>
    [HttpGet("customers/{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var result = await Mediator.Send(new GetCustomerByIdQuery());
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to update customer record
    /// </summary>
    /// <param name="customer">
    /// customer contains the updated customers's data
    /// </param>
    /// <returns> 
    /// Customer id of updated record 
    /// </returns>
    [HttpPut("customers/{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerRequestBody)
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = id,
            CustomerName = customerRequestBody.CustomerName,
            PhoneNumber = customerRequestBody.PhoneNumber
        };

        var updatedCustomerId = await Mediator.Send(command);

        return Ok(updatedCustomerId);
    }
}







