using eInsuranceApp.Entities.CustomerDTO;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface ICustomerRL
    {
        Task<CustomerEntity> RegisterCustomerAsync(CustomerEntity customer);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<bool> IsAgentExistsAsync(int agentId);
        Task<CustomerEntity> GetCustomerByIdAsync(int customerId);


    }
}
