using Ebay.Domain.Entities.JoinTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Mappings
{
    public class ProductCategoryMapping
    {
        public ProductCategoryMapping(EntityTypeBuilder<ProductCategory> modelBuilder)
        {
            modelBuilder
                .HasKey(bc => bc.Id);
            /*modelBuilder
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductCategories)
                .HasForeignKey(bc => bc.ProductId);
            modelBuilder
                .HasOne(bc => bc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(bc => bc.CategoryId);*/
        }
    }
}
