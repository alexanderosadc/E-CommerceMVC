using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities.JoinTables
{
    public class ProductDiscount
    {
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
