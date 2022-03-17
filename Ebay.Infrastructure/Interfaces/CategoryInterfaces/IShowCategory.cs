using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.CategoryInterfaces
{
    public interface IShowCategory
    {
        public Task<IEnumerable<CategoryViewDTO>> GetCategoyDTO();
    }
}
