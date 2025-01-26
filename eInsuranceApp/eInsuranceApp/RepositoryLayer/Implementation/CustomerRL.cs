using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class CustomerRL : ICustomerRL
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CustomerRL> _logger;

        public CustomerRL(AppDbContext dbContext, ILogger<CustomerRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CustomerEntity> RegisterCustomerAsync(CustomerEntity customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
            return customer;

        }

        public async Task<bool> IsAgentExistsAsync(int agentId)
        {
            return await _dbContext.Agents.AnyAsync(x => x.AgentID == agentId);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _dbContext.Customers.AnyAsync(x => x.Email == email);
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(int customerId)
        {
            //return await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerID == customerId);
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerID == customerId);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {customerId} not found.");
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                throw;
            }
        }

    }
}
