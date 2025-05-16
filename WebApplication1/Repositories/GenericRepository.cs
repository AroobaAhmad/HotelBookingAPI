using HotelBookingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBookingAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
      
        protected readonly AppDbContext _context; // repository uses appdbcontext 
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        } 

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
        public async Task DeleteAllAsync()
        {
            var allEntities = await _dbSet.ToListAsync(); 
            _dbSet.RemoveRange(allEntities);
           await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
