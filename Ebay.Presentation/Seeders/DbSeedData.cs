using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ebay.Presentation.Seeders
{
    public static class DbSeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            DiscountPopulate(context);
            CategoryPopulate(context);

            context.SaveChanges();
        }

        public static void DiscountPopulate(AppDbContext context)
        {

            if (!context.Discounts.Any())
            {
                context.Discounts.AddRange(
                    new Discount
                    {
                        Name = "Christmas discount",
                        DiscountPercent = 12,
                        IsActive = true,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(5),
                    },
                    new Discount
                    {
                        Name = "Passover discount",
                        DiscountPercent = 12,
                        IsActive = true,
                        StartDate = DateTime.Now.AddMonths(2),
                        EndDate = DateTime.Now.AddMonths(2).AddDays(5),
                    }
                    );
            }
        }

        public static void CategoryPopulate(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "RealEstate",
                        Description = "Real estate includes all properties on the ground.",
                    },
                    new Category
                    {
                        Name = "Apartment",
                        Description = "Place to live as one cell of the big building.",
                    },
                    new Category
                    {
                        Name = "House",
                        Description = "Building on the ground where all surrounding ground is yours.",
                    }
                    );
            }
        }
    }
}
