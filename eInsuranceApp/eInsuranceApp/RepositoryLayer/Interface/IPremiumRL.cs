using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IPremiumRL
    {
        Task<decimal> CalculatePremiumAsync(PremiumCalculationDTO premiumDetails);
        //Task<decimal> CalculatePremiumAsync(int customerId, int policyId, int schemeId, decimal baseRate, int customerAge);
        Task<PremiumCalculationDTO> GetPremiumRateAsync(int schemeId);
        Task<Policy> GetPolicyByIdAsync(int policyId);
        Task<PremiumCalculationDTO> GetPremiumBySchemeIdAsync(int schemeId);
        Task<PremiumCalculationDTO> GetPremiumByIdAsync(int premiumId);


    }
}
