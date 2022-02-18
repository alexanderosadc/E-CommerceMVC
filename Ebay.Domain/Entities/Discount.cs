
using Ebay.Domain.Entities.Base;

namespace Ebay.Domain.Entities
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}