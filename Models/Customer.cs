using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI.Models
{
    public class Customer
    {
        [Key]
        public string id { get; set; }
        public string customerId { get; set; }
        public string category { get; set; }//partition key
        public string name { get; set; }
        public string email { get; set; }
    }
}
