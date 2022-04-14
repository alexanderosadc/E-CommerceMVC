using Ebay.Domain.Entities;
using Ebay.Infrastructure.CustomAttributes;

namespace Ebay.Infrastructure.ViewModels.Admin.Index
{
    public class ProductViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalQuantity { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }

        public IEnumerable<string>? CategoryNames { get; set; }

        public IEnumerable<DiscountViewDTO>? Discounts { get; set; }
        
        public IEnumerable<Photo> Photos;
    }
}
