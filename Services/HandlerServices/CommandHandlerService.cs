using HotelBookingAPI.Commands;
using HotelBookingAPI.Models;
using HotelBookingAPI.Repositories;

namespace HotelBookingAPI.Services.HandlerServices
{
    public class CommandHandlerService(IUnitOfWork unitOfWork)
    {
        public async Task<bool> AddNewCustomer(Command command)
        {

            try
            {
                await unitOfWork.Customers.AddAsync(command._customer);
                await unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                return await unitOfWork.Customers.UpdateAsync(customer);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteCustomerAsync(string id)
        {
            try
            {
                var customer = await unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                return false;

            await unitOfWork.Customers.DeleteAsync(customer);
            await unitOfWork.SaveChangesAsync();
            return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteAllCustomerAsync()
        {
            try
            {
                await unitOfWork.Customers.DeleteAllAsync();
                await unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
