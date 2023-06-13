using Microsoft.AspNetCore.Mvc;
using RetailStore.Model;
using RetailStore.Repository;

[ApiController]
[Route("[controller]")]
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
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await customerRepository.GetAll();
        return Ok(customers);
    }

    /// <summary>
    /// Adding customer data to the database
    /// </summary>
    /// <returns>
    /// Id of inserted record
    /// </returns>    
    [HttpPost]
    public async Task<IActionResult> AddCustomer(Customer customer)
    {
        var createdCustomer = await customerRepository.Create(customer);
        return Ok(createdCustomer.Id);
    }

    /// <summary>
    /// Endpoint to delete a customer by ID.
    /// </summary>
    /// <param name="id">Customer's Id to fetch Customer's data</param>
    [HttpDelete("customer/{id}")]
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
    [HttpGet("customer/{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
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
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer(Customer customer)
    {
        var updatedCustomer = await customerRepository.Update(customer);
        return Ok(updatedCustomer.Id);
    }
}
