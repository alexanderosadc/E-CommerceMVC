using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.Services
{
    public interface IProductCategoryService
    {
        public List<ProductCategory> CreateProductCategories(List<Category> categories, Product product);
        public Task DeleteAll(int productId);
    }
}
