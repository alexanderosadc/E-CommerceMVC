using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.Index
{
    public class DiscountViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
