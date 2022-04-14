using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.CategoryInterfaces
{
    public interface ICreateCategory
    {
        public CategoryViewDTO CreateCategoryView(Category category);
        public Task CreateNewCategory(CategoryCreateDTO categoryViewModel);
        public Task<CategoryCreateDTO> GetCategoryCreateDTO();
        public Task<List<Category>> GetChildCategories(CategoryCreateDTO dto);
    }
}
