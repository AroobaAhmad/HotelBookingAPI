using HotelBookingAPI.Models;
using Microsoft.Azure.Cosmos;

namespace HotelBookingAPI.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _container.CreateItemAsync(customer, new PartitionKey(customer.category));
        }

        public async Task<IEnumerable<Customer>> GetCustomerAsync(string customerId)
        {
            var query = _container.GetItemQueryIterator<Customer>(
                new QueryDefinition("SELECT * FROM c WHERE c.customerId = @customerId")
                    .WithParameter("@customerId", customerId));
            var results = new List<Customer>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

    }
}
