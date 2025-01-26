using eInsuranceApp.Entities.Dtos;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IPolicyBL
    {
        Task CreatePolicyAsync(Policy policyDTO);
        Task<Policy> GetPolicyAsync(int policyId);
        Task<decimal?> GetBasePremiumRateAsync(int schemeId);
        //Task<decimal> GetBasePayRateByPolicyIdAsync(int policyId);
    }
}

