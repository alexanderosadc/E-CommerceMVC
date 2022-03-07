using Ebay.Domain.Entities.Base;
using Ebay.Domain.Entities.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int? ParentId { get; set; }
        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
