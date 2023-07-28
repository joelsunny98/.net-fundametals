using Microsoft.AspNetCore.Mvc;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;

namespace RetailStore.Controllers;

/// <summary>
/// Controller for managing products of Retailstore
/// </summary>
[ApiController]
[Route("api")]
public class ProductController : BaseController
{

    /// <summary>
    /// Endpoint to fetch details of an product.
    /// </summary>
    /// <returns>It returns customer details</returns>
    [HttpGet("products")]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
        var products = await Mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    /// <summary>
    /// Adding product data to the database
    /// </summary>
    /// <param name="product"></param>
    /// <returns>
    /// Id of inserted record
    /// </returns>    
    [HttpPost("products")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddProduct(AddProductCommand product)
    {
        var result = await Mediator.Send(product);
        return Ok(result);
    }


    /// <summary>
    /// Endpoint to delete a product by ID.
    /// </summary>
    /// <param name="id">product's Id to fetch product's data</param>
    [HttpDelete("products/{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deletedProductId = await Mediator.Send(new DeleteProductCommand { ProductId = id });
        return Ok(deletedProductId);
    }

    /// <summary>
    /// Endpoint to fetch details of an product with given id.
    /// </summary>
    /// <param name="id">Product's Id to fetch product's data</param>
    [HttpGet("products/{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await Mediator.Send(new GetProductByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to update product record
    /// </summary>
    /// <param name="command">
    /// Product contains the updated products's data
    /// </param>
    /// <returns> 
    /// Product id of updated record 
    /// </returns>
    [HttpPut("product")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
    {
        var updatedProductId = await Mediator.Send(command);
        return Ok(updatedProductId);
    }

    /// <summary>
    /// Endpoint to generate a barcode for product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("products/{id}/barcode")]
    [ProducesResponseType(typeof(Image), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductBarcode([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetProductBarcodeQuery { ProductId = id });
        return Ok(result);
    }
}