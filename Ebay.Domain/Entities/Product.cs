
using Ebay.Domain.Entities.Base;

namespace Ebay.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            Categories = new List<Category>();
            Discounts = new List<Discount>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalQuantity { get; set; }
        public double Price { get; set; }
        public ICollection<Category> Categories { get; private set; }
        public ICollection<Discount> Discounts { get; private set; }
    }
}
