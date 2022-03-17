using Ebay.Infrastructure.Interfaces.ProductInterfaces;
using Ebay.Infrastructure.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces
{
    public interface IProductBL : ICreateProduct, IEditProduct, IShowProducts, IDeleteEntity
    {
       
    }
}
