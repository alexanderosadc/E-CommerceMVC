using Ebay.Domain.Entities;

namespace Ebay.Infrastructure.ViewModels.Admin.Index
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalQuantity { get; set; }
        public double Price { get; set; }

        public List<string> CategoryNames;

        public List<int> discountPercentages;
    }
}
