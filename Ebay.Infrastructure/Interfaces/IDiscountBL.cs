using Ebay.Infrastructure.Interfaces.DiscountInterfaces;
using Ebay.Infrastructure.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces
{
    public interface IDiscountBL : ICreateDiscount, IEditDiscount, IShowDiscount, IDeleteEntity
    {
    }
}
