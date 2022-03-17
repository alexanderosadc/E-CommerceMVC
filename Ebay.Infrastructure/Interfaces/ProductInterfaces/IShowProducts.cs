using Ebay.Infrastructure.ViewModels.Admin.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.ProductInterfaces
{
    public interface IShowProducts
    {
        public Task<ProductViewDTO> GetProductView(int id);
        public Task<IEnumerable<ProductViewDTO>> GetProductsViews();
    }
}
