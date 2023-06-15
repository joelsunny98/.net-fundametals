﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RetailStore.Controllers
{
    [ApiController]
    [Route("api/products")]
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
        /// <returns>It returns employee details</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productRepository.GetAll();
            return Ok(products);
        }

        /// <summary>
        /// Adding product data to the database
        /// </summary>
        /// <returns>
        /// Id of inserted record
        /// </returns>    
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var createdProduct = await productRepository.Create(product);
            return Ok(createdProduct.Id);
        }

        /// <summary>
        /// Endpoint to delete a product by ID.
        /// </summary>
        /// <param name="id">product's Id to fetch product's data</param>
        [HttpDelete("{id}")]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
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
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var updatedProduct = await productRepository.Update(product);
            return Ok(updatedProduct.Id);
        }
    }
}