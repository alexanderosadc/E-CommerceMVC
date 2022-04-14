using Ebay.Infrastructure.Interfaces.AdminPresentation.ProductInterfaces;
using Ebay.Infrastructure.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation
{
    public interface IProductBL : ICreateProduct, IEditProduct, IShowProducts, IDeleteEntity
    {
       
    }
}
