using Ebay.Domain.Entities.Base;
using Ebay.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ebay.Infrastructure.Repository
{
    internal class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDbContext _context;
        private DbSet<T> _entities;

        public Repository(IDbContext context)
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
            await _context.SaveChangesAsync();
        }
    }
}
