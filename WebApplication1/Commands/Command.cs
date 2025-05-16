using HotelBookingAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HotelBookingAPI.Commands
{
    public class Command
    {
        public Customer _customer { get; set; }

        public Command(Customer? customer)
        {
            _customer = new Customer
            {
                name = customer.name,
                category = customer.category,
                id = customer.id,
                customerId = customer.customerId
            };
        }
    }
}
