using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class CustomerRL : ICustomerRL
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CustomerRL> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CustomerRL(AppDbContext dbContext, ILogger<CustomerRL> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
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

        public async Task<List<PolicyViewDTO>> GetPoliciesByCustomerIdAsync(int customerId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var policies = new List<PolicyViewDTO>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetPoliciesByCustomerId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@CustomerID", customerId));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var policy = new PolicyViewDTO
                                {
                                    PolicyID = reader.GetInt32(reader.GetOrdinal("PolicyID")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                    SchemeName = reader.GetString(reader.GetOrdinal("SchemeName")),
                                    SchemeDetails = reader.GetString(reader.GetOrdinal("SchemeDetails")),
                                    PolicyDetails = reader.GetString(reader.GetOrdinal("PolicyDetails")),
                                    DateIssued = reader.GetDateTime(reader.GetOrdinal("DateIssued")),
                                    MaturityPeriod = reader.GetInt32(reader.GetOrdinal("MaturityPeriod")),
                                    PolicyLapseDate = reader.GetDateTime(reader.GetOrdinal("PolicyLapseDate")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                };

                                policies.Add(policy);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching policies for CustomerID: {CustomerID}," , customerId);
                throw;
            }

            return policies;
        }

    }
}
