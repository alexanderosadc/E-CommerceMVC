using Ebay.Infrastructure.Interfaces.CategoryInterfaces;
using Ebay.Infrastructure.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces
{
    public interface ICategoryBL: ICreateCategory, IEditCategory, IShowCategory, IDeleteEntity
    {
    }
}
