using Ebay.Domain.Entities.Base;
using Ebay.Domain.Entities.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } 
    }
}
