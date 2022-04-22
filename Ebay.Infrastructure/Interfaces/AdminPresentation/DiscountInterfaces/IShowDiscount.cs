using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.DiscountInterfaces
{
    public interface IShowDiscount
    {
        public Task<DiscountViewDTO> GetDiscountDTO();
        public Task<DiscountViewDTO> GetDiscountDTO(int id);
        public Task<IEnumerable<DiscountViewDTO>> GetDiscountsDTO();
        public Task<List<SelectListItem>> GetDropdownDiscounts();
    }
}
