using Ebay.Infrastructure.ViewModels.Admin.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.DiscountInterfaces
{
    public interface IEditDiscount
    {
        public Task<DiscountViewDTO> EditDiscount(int itemId);
        public Task UpdateDiscount(DiscountViewDTO discountViewModel);
        
    }
}
