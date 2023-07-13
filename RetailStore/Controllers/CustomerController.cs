﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Features.CustomerManagement.Queries;
using RetailStore.Features.OrderManagement.Commands;
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

    [HttpPost("customers")]
    public async Task<IActionResult> AddCustomer(CustomerDto customer)
    {
        var command = new AddCustomerCommand
        {
            CustomerName = customer.CustomerName,
            PhoneNumber = customer.PhoneNumber
        };

        var result = await Mediator.Send(command);
        return Ok(result);
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
        var deletedCustomer = await _customerRepository.Delete(id);
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
        var customer = await _customerRepository.GetById(id);
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
        var updatedCustomer = await _customerRepository.Update(customer);
        return Ok(updatedCustomer.Id);

    }
}

