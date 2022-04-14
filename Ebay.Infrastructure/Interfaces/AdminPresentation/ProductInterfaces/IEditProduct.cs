using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.ProductInterfaces
{
    public interface IEditProduct
    {
        public Task<ProductCreateDTO> GetEditProductView(int itemId);
        public Task UpdateProduct(ProductCreateDTO dto);
        public Task DeletePhoto(string id);
    }
}
