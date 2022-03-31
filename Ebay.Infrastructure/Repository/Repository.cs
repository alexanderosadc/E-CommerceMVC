using Ebay.Domain.Entities.Base;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ebay.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private DbSet<T> _entities;

        public Repository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        public async Task Delete(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            var result = await _entities.SingleOrDefaultAsync(item => item.Id == id);
            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            // Verify entity not tracked
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetFirstValues(int pageIndex, int pageSize)
        {
            return _entities.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public async Task<int> GetNumberOfItems()
        {
            return await _entities.CountAsync();
        }
    }
}
