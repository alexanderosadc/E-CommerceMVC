
using Ebay.Domain.Entities.Base;

namespace Ebay.Domain.Entities
{
    public class CategoryField : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}