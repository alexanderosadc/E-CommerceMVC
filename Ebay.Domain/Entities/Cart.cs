using Ebay.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public double TotalSum { get; set; }
        public virtual User User { get; set; }
    }
}
