using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.ProductInterfaces
{
    public interface IShowProducts
    {
        public Task<ProductViewDTO> GetProductView(int id);
        public Task<ProductViewListDTO> GetProductsViews(int currentPageNumber, int pageSize = 2);
        public Task<ProductViewListDTO> GetProductsViewsByCategoryDiscounts(int currentPageNumber, int? categoryId,
            int? discountId,
            int pageSize = 2);

        public List<Product> GetProductsByCategoryId(int categoryId, List<Product> products);
        public List<Product> GetProductsByDiscountId(int discountId, List<Product> products);
        public List<Product> GetCombinedListOfProducts(int? categoryId, int? discountId, List<Product> products);
        public List<ProductViewDTO> GetProductsWithFinalPrice(List<Product> products);
        public IEnumerable<ProductViewDTO> SortProductViews(string sortCondition, IEnumerable<ProductViewDTO> products);
        public IEnumerable<ProductViewDTO> SearchProductByName(string name, IEnumerable<ProductViewDTO> products);
    }
}
