using Microsoft.EntityFrameworkCore;
using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Domain.Mappings;
using Ebay.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ebay.Infrastructure.Persistance
{
    public class AppDbContext : IdentityDbContext<AppUser>, IDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new CategoryFieldMapping(modelBuilder.Entity<CategoryField>());
            new CategoryMapping(modelBuilder.Entity<Category>());
            new DiscountMapping(modelBuilder.Entity<Discount>());
            new ProductMapping(modelBuilder.Entity<Product>());
            new CartItemMapping(modelBuilder.Entity<CartItem>());
            new UserCartMapping(modelBuilder.Entity<UserCart>());
        }

        DbSet<CategoryField> CategoryFields { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet <Product> Products { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<UserCart> UserCarts { get; set; }
    }
}
