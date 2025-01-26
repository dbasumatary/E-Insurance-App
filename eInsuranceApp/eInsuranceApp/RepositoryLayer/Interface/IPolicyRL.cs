using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IPolicyRL
    {
        Task<int> ValidateAndCreatePolicyAsync(Policy policyDTO);
        Task<Policy> GetPolicyByIdAsync(int policyId);
        Task<decimal?> GetBasePremiumRateAsync(int schemeId);
    }
}
