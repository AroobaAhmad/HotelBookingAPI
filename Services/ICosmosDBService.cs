using HotelBookingAPI.Models;

namespace HotelBookingAPI.Services
{
    public interface ICosmosDbService
    {
        //  Task<IEnumerable<HotelBooking>> GetAllHotelBookings();
        Task AddCustomerAsync(Customer hotelBooking);
        Task<IEnumerable<Customer>> GetCustomerAsync(string customerId);
    }
}
