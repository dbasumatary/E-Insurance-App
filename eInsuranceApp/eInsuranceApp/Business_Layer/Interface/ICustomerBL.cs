using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface ICustomerBL
    {
        Task<CustomerRegistrationResponse> RegisterCustomerAsync(CustomerRegistrationRequest customerDTO);
        Task<int> GetCustomerAgeAsync(int customerId);
        Task<List<PolicyViewDTO>> GetPoliciesForCustomerAsync(int customerId);
    }
}
