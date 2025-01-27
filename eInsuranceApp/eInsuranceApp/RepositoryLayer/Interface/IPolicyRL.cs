using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IPolicyRL
    {
        Task<int> ValidateAndCreatePolicyAsync(Policy policyDTO);
        //Task<Policy> GetPolicyByIdAsync(int policyId);
        Task<decimal?> GetBasePremiumRateAsync(int schemeId);
        Task<PolicyViewDTO> GetPolicyDetailsByIdAsync(int policyId);
        //Task<IEnumerable<Policy>> Search(int? policyId, string customerName);
        Task<IEnumerable<PolicyViewDTO>> GetPoliciesAsync();
        Task<PolicyViewDTO> SearchPolicyDetailsAsync(int? policyId, string customerName);
    }
}
