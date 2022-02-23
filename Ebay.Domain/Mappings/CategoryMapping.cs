using Ebay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Mappings
{
    public class CategoryMapping
    {
        public CategoryMapping(EntityTypeBuilder<Category> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasMany(item => item.Categories)
                .WithOne(item => item.Parent)
                .HasForeignKey(item => item.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
