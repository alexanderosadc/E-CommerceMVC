using Ebay.Infrastructure.Interfaces.AdminPresentation.CategoryInterfaces;
using Ebay.Infrastructure.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation
{
    public interface ICategoryBL: ICreateCategory, IEditCategory, IShowCategory, IDeleteEntity
    {
    }
}
