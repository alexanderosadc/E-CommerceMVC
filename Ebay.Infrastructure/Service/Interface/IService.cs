using Ebay.Infrastructure.Service.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Service.Interface
{
    public interface IService<T> where T : BaseService
    {
        public Task<int> GetNumberOfRecords();
        public Task<IEnumerable<T>> GetAllRecords();
        public Task<IEnumerable<T>> GetRecordById(int id);
        public Task<IEnumerable<T>> GetRecordByName(string name);
    }
}
