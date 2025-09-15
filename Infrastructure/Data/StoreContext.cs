using System;
using Core.Entities;
using Infrastructure.Configs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class StoreContext:DbContext
	{
		public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
    }
}

