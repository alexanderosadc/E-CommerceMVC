using Ebay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
