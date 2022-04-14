using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.ProductInterfaces
{
    public interface ICreateProduct
    {
        public Task PostCreateProductDTO(ProductCreateDTO dto);
        public Task<ProductCreateDTO> GetProductCreateView();
        public Task<Product> CreateProductForDb(ProductCreateDTO productCreateViewModel, bool isProductForInserting);
    }
}
