using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing products of Retailstore
/// </summary>
[ApiController]
[Route("api")]
public class ProductController : ControllerBase
{
    private readonly IRepository<Product> productRepository;

    public ProductController(IRepository<Product> _productRepository)
    {
        productRepository = _productRepository;
    }

    /// <summary>
    /// Endpoint to fetch details of an product.
    /// </summary>
    /// <returns>It returns customer details</returns>
    [HttpGet("products")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
        var products = await productRepository.GetAll();
        var productsResponse = products.Select(e => new ProductDto
        {
            ProductName = e.Name,
            ProductPrice = e.Price
        });
        return Ok(productsResponse);
    }

    /// <summary>
    /// Adding product data to the database
    /// </summary>
    /// <returns>
    /// Id of inserted record
    /// </returns>    
    [HttpPost("products")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddProduct(ProductDto productRequestBody)
    {
        var duplicateProduct = await productRepository.Find(x => x.Name == productRequestBody.ProductName);
        if (duplicateProduct.Any())
        {
            return BadRequest("Product with same name already exists");
        }
        else
        {
            var product = new Product
            {
                Name = productRequestBody.ProductName,
                Price = productRequestBody.ProductPrice,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            var createdProduct = await productRepository.Create(product);
            return Ok(createdProduct.Id);
        }
    }

    /// <summary>
    /// Endpoint to delete a product by ID.
    /// </summary>
    /// <param name="id">product's Id to fetch product's data</param>
    [HttpDelete("products/{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deletedProduct = await productRepository.Delete(id);
        if (deletedProduct == null)
        {
            return NotFound();
        }

        return Ok(deletedProduct.Id);
    }

    /// <summary>
    /// Endpoint to fetch details of an product with given id.
    /// </summary>
    /// <param name="id">Product's Id to fetch product's data</param>
    [HttpGet("products/{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        var productResponse = new ProductDto
        {
            ProductName = product.Name,
            ProductPrice = product.Price
        };

        return Ok(productResponse);
    }

    /// <summary>
    /// Endpoint to update product record
    /// </summary>
    /// <param name="product">
    /// Product contains the updated products's data
    /// </param>
    /// <returns> 
    /// Product id of updated record 
    /// </returns>
    [HttpPut("products/{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto productRequestBody)
    {
        var product = new Product
        {
            Id = id,
            Name = productRequestBody.ProductName,
            Price = productRequestBody.ProductPrice,
            UpdatedOn = DateTime.UtcNow
        };

        var updatedProduct = await productRepository.Update(product);
        return Ok(updatedProduct.Id);
    }
}