﻿using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.CategoryInterfaces
{
    public interface IEditCategory
    {
        public Task<CategoryCreateDTO> GetCategoryEditDTO(int itemId);
        public Task UpdateCategory(CategoryCreateDTO categoryCreateViewModel);
    }
}
