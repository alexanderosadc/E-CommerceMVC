using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.CategoryInterfaces
{
    public interface IEditCategory
    {
        public Task<CategoryCreateDTO> EditCategory(int itemId);
        public Task UpdateCategory(CategoryCreateDTO categoryCreateViewModel);
    }
}
