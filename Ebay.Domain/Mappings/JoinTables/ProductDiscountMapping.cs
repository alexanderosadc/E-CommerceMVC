using Ebay.Domain.Entities.JoinTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Mappings.JoinTables
{
    public class ProductDiscountMapping
    {
        public ProductDiscountMapping(EntityTypeBuilder<ProductDiscount> modelBuilder)
        {
            modelBuilder
                .HasKey(bc => bc.Id);
            /*modelBuilder
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductDiscounts)
                .HasForeignKey(bc => bc.ProductId);
            modelBuilder
                .HasOne(bc => bc.Discount)
                .WithMany(c => c.ProductDiscounts)
                .HasForeignKey(bc => bc.DiscountId);*/
        }
    }
}
