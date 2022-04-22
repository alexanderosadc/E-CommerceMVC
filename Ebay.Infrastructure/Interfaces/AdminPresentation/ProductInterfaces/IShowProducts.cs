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
    }
}
