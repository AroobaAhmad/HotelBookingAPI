using HotelBookingAPI.Models;

namespace HotelBookingAPI.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Customer> Customers { get; }
        Task<int> SaveChangesAsync();
    }
}
