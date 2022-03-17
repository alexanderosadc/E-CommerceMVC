using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.Shared
{
    public interface IDeleteEntity
    {
        public Task Delete(int id);
    }
}
