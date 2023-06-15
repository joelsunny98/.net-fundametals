using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
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
public class CustomerController : ControllerBase
{
    private readonly IRepository<Customer> customerRepository;
    public CustomerController(IRepository<Customer> _customerRepository)
    {
        customerRepository = _customerRepository;
    }

    /// <summary>
    /// Endpoint to fetch details of an customer.
    /// </summary>
    /// <returns>It returns customer details</returns>
    [HttpGet("customers")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await customerRepository.GetAll();
        var responseCustomers = customers.Select(e => new CustomerDto
        {
            CustomerName = e.Name,
            PhoneNumber = (long)e.PhoneNumber
        });

        return Ok(responseCustomers);
    }

    /// <summary>
    /// Adding customer data to the database
    /// </summary>
    /// <returns>
    /// Id of inserted record
    /// </returns>    
    [HttpPost("customers")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCustomer(CustomerDto customerRequestBody)
    {
        var phoneNumber = customerRequestBody.PhoneNumber.ToString();
        var duplicateCustomer = await customerRepository.Find(x => x.PhoneNumber == customerRequestBody.PhoneNumber);

        if (phoneNumber.Length !=10)
        {
            return BadRequest("Phonenumber Must be 10 digits");
        }
        else if (duplicateCustomer.Any())
        {
            return BadRequest("Phonenumber Must be unique");
        }
        else
        {
            var customer = new Customer
            {
                Name = customerRequestBody.CustomerName,
                PhoneNumber = customerRequestBody.PhoneNumber,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            var createdCustomer = await customerRepository.Create(customer);
            return Ok(createdCustomer.Id);
        }
    }

    /// <summary>
    /// Endpoint to delete a customer by ID.
    /// </summary>
    /// <param name="id">customers's Id to fetch customers's data</param>
    [HttpDelete("customers/{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var deletedCustomer = await customerRepository.Delete(id);
        if (deletedCustomer == null)
        {
            return NotFound();
        }

        return Ok(deletedCustomer.Id);
    }

    /// <summary>
    /// Endpoint to fetch details of an customer with given id.
    /// </summary>
    /// <param name="id">Customers's Id to fetch customer's data</param>
    [HttpGet("customers/{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound();
        }

        var customerResponse = new CustomerDto
        {
            CustomerName = customer.Name,
            PhoneNumber = (long)customer.PhoneNumber
        };

        return Ok(customerResponse);
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
        var customer = new Customer
        {
            Id = id,
            Name = customerRequestBody.CustomerName,
            PhoneNumber = customerRequestBody.PhoneNumber,
            UpdatedOn = DateTime.UtcNow
        };
        var updatedCustomer = await customerRepository.Update(customer);
        return Ok(updatedCustomer.Id);

    }
}

