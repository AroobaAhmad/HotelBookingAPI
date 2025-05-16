using HotelBookingAPI.Models;
using HotelBookingAPI.Repositories;

namespace HotelBookingAPI.Services.Handlers
{
    public class QueryHandlerService
    {
        public IGenericRepository<Customer> Customers { get; private set; }

        public QueryHandlerService(IGenericRepository<Customer> customer)
        {
            Customers = customer;
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await Customers.GetByIdAsync(id);
        } 
        public async Task<IEnumerable<Customer>> GetByCategoryAsync(string category)
        {
            return await Customers.FindAsync(c=>c.category == category);
        }
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await Customers.GetAllAsync();
        }
    }
}
