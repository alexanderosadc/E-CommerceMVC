using Ebay.Infrastructure.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.Index
{
    public class DiscountViewDTO
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Introduce Username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Introduce Discount Percentage")]
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; }

        [DateStart]
        public DateTime StartDate { get; set; }  = DateTime.Now.Date.AddHours(DateTime.Now.Hour) + TimeSpan.FromHours(1);

        [DateEnd(DateStartPropertyName = "StartDate")]
        public DateTime EndDate { get; set; } = DateTime.Now.Date.AddHours(DateTime.Now.Hour) + TimeSpan.FromHours(2);
    }
}
