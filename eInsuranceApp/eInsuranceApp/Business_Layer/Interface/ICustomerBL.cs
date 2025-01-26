using eInsuranceApp.Entities.CustomerDTO;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface ICustomerBL
    {
        Task<CustomerRegistrationResponse> RegisterCustomerAsync(CustomerRegistrationRequest customerDTO);
        Task<int> GetCustomerAgeAsync(int customerId);
    }
}
