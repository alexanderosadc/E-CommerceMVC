using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.CategoryInterfaces
{
    public interface IShowCategory
    {
        public Task<IEnumerable<CategoryViewDTO>> GetCategoyDTO();
        public Task<List<SelectListItem>> GetDropdownCategories();
    }
}
