using Ebay.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities.JoinTables
{
    public class ProductDiscount : BaseEntity
    {
        public int DiscountId { get; set; }
        public virtual Discount Discount { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
