using Microsoft.EntityFrameworkCore;
using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Domain.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Mappings.JoinTables;

namespace Ebay.Infrastructure.Persistance
{
    public class AppDbContext : IdentityDbContext<User>, IDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new ProductDiscountMapping(modelBuilder.Entity<ProductDiscount>());
            new ProductCategoryMapping(modelBuilder.Entity<ProductCategory>());

            new CartItemMapping(modelBuilder.Entity<CartItem>());
            new CartMapping(modelBuilder.Entity<Cart>());
            new CategoryMapping(modelBuilder.Entity<Category>());
            new DiscountMapping(modelBuilder.Entity<Discount>());
            new PhotoMapping(modelBuilder.Entity<Photo>());
            new ProductMapping(modelBuilder.Entity<Product>());
            new UserMapping(modelBuilder.Entity<User>());

        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }

    }
}
