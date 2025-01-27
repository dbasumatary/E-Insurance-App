using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface ICustomerRL
    {
        Task<CustomerEntity> RegisterCustomerAsync(CustomerEntity customer);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<bool> IsAgentExistsAsync(int agentId);
        Task<CustomerEntity> GetCustomerByIdAsync(int customerId);

        Task<List<PolicyViewDTO>> GetPoliciesByCustomerIdAsync(int customerId);
    }
}
