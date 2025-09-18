﻿using System;
using Core.Entities;
using Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class ProductRepository:IProductRepository
	{
        private readonly StoreContext context;

        public ProductRepository(StoreContext context)
        {
            this.context = context;
        }

        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChnagesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }
    }
}

