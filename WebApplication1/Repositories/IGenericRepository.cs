using HotelBookingAPI.Models;
using HotelBookingAPI.Repositories;
using System.Linq.Expressions;

namespace HotelBookingAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);

        Task DeleteAsync(T entity);
        Task DeleteAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
