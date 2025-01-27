using eInsuranceApp.Entities.Dtos;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IPolicyBL
    {
        Task CreatePolicyAsync(Policy policyDTO);
        //Task<Policy> GetPolicyAsync(int policyId);
        Task<decimal?> GetBasePremiumRateAsync(int schemeId);
        //Task<decimal> GetBasePayRateByPolicyIdAsync(int policyId);
        Task<PolicyViewDTO> GetPolicyDetailsByIdAsync(int policyId);
        //Task<IEnumerable<Policy>> SearchPolicies(int? policyId, string customerName, string policyDetails, string status);
        //Task<IEnumerable<PolicyViewDTO>> SearchPoliciesAsync(int? policyId, string customerName);
        Task<IEnumerable<PolicyViewDTO>> GetPoliciesAsync(); //for search
        Task<PolicyViewDTO> SearchPolicyDetailsAsync(int? policyId, string customerName);
    }
}

