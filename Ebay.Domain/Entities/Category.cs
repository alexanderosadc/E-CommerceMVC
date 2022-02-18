
using Ebay.Domain.Entities.Base;

namespace Ebay.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            CategoryFields = new List<CategoryField>();
            Categories = new List<Category>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Categories { get; private set; }
        public ICollection<CategoryField> CategoryFields { get; private set; }
    }
}