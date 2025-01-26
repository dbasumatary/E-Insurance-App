using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IPremiumBL
    {
        Task<decimal> CalculatePremiumAsync(PremiumCalculationDTO premiumDetails);
        //Task<decimal> GetCalculatedPremiumAsync(int customerId, int policyId, int schemeId, decimal baseRate, int customerAge);
        Task<Policy> GetPolicyByIdAsync(int policyId);
    }
}
