using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.Index
{
    public class ProductViewListDTO
    {
        public IEnumerable<ProductViewDTO> Products { get; set; }
        public PagingInfo PaginationInfo { get; set; }
    }
}
