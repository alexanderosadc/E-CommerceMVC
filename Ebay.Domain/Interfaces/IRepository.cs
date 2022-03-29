﻿
using Ebay.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> Get(int id);
        public Task Insert(T entity);
        public Task Update(T entity);
        public Task Delete(T entity);
    }
}
