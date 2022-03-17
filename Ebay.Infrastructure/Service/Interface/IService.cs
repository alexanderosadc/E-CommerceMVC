using Ebay.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Service.Interface
{
    public interface IService<T> where T : BaseEntity
    {
        public Task<int> GetNumberOfRecords();
        public Task<IEnumerable<T>> GetAllRecords();
        public Task<IEnumerable<T>> GetRecordById(int id);
        public Task<IEnumerable<T>> GetRecordByName(string name);
    }
}
