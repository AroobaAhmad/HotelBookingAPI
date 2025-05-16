using HotelBookingAPI.Commands;
using HotelBookingAPI.Models;
using HotelBookingAPI.Repositories;
using HotelBookingAPI.Services;
using HotelBookingAPI.Services.Handlers;
using HotelBookingAPI.Services.HandlerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly QueryHandlerService _queryHandlerService;
        private readonly CommandHandlerService _commandHandlerService;

        public CustomerController(QueryHandlerService queryHandlerService, CommandHandlerService commandHandlerService, IUnitOfWork unitOfWork)
        {
            _queryHandlerService = queryHandlerService;
            _commandHandlerService = commandHandlerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer? customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.id) || string.IsNullOrEmpty(customer.category))
            {
                return BadRequest("Customer data is missing required fields.");
            }

            bool isCustomerAdded = await _commandHandlerService.AddNewCustomer(new Command(customer));
            if (isCustomerAdded)
            {
                return Created();
            }
            else
            {
                return BadRequest("Failed to add the customer to the database.");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] string? id, [FromQuery] string? category)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var customer = await _queryHandlerService.GetByIdAsync(id);
                return customer == null ? NotFound($"Customer with id '{id}' not found.") : Ok(new[] { customer });
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var customers = await _queryHandlerService.GetByCategoryAsync(category);
                return !customers.Any()
                    ? NotFound($"No customers found in category '{category}'.")
                    : Ok(customers);
            }

            var allCustomers = await _queryHandlerService.GetAllCustomersAsync();
            return allCustomers.Any() ? Ok(allCustomers) : NotFound("No customers found.");
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateCustomer([FromQuery] string id, [FromBody] Customer updatedData)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Customer ID is required.");

            var existingCustomer = await _queryHandlerService.GetByIdAsync(id);
            if (existingCustomer == null)
                return NotFound($"Customer with ID '{id}' not found.");

            if (!string.IsNullOrWhiteSpace(updatedData.name))
                existingCustomer.name = updatedData.name;

            if (!string.IsNullOrWhiteSpace(updatedData.customerId))
                existingCustomer.customerId = updatedData.customerId;

            if (!string.IsNullOrWhiteSpace(updatedData.category))
                existingCustomer.category = updatedData.category;

            var success = await _commandHandlerService.UpdateCustomerAsync(existingCustomer);
            if (!success)
                return StatusCode(500, "Failed to update the customer.");
            existingCustomer = await _queryHandlerService.GetByIdAsync(id);
            return Ok(existingCustomer); // 200 OK with updated customer
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer([FromQuery] string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Please provide a customer ID to delete.");

            var result = await _commandHandlerService.DeleteCustomerAsync(id);
            return result ? NoContent() : NotFound($"Customer with id '{id}' not found.");
        }
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllCustomers()
        {
            var result = await _commandHandlerService.DeleteAllCustomerAsync();
            return result ? NoContent() : NotFound("No customers to delete.");
        }
    }
}
