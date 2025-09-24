﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase 
    {
        private readonly IProductRepository repo;
        public ProductsController(IProductRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type,string? sort)
        {
            return Ok(await repo.GetProductsAsync(brand,type,sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);

            if (await repo.SaveChnagesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("problem creating product.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Cannot update the product.");
            repo.UpdateProduct(product);
            if(await repo.SaveChnagesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product.Id == null) return NotFound();

            repo.DeleteProduct(product);

            if (await repo.SaveChnagesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product.");

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await repo.GetBrandAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await repo.GetTypeAsync());
        }

        private bool ProductExists(int id)
        {
            return repo.ProductExists(id);
        }
    }
}