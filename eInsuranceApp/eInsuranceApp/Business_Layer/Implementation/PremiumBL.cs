using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Implementation;
using eInsuranceApp.RepositoryLayer.Interface;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class PremiumBL : IPremiumBL
    {
        private readonly IPremiumRL _premiumRL;
        private readonly ILogger<PaymentRL> _logger;

        public PremiumBL(IPremiumRL premiumRL, ILogger<PaymentRL> logger)
        {
            _premiumRL = premiumRL;
            _logger = logger;
        }

        /*public async Task<decimal> GetCalculatedPremiumAsync(int customerId, int policyId, int schemeId, decimal baseRate, int customerAge)
        {
            try
            {
                return await _premiumRL.CalculatePremiumAsync(customerId, policyId, schemeId, baseRate, customerAge);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating premium.");
                throw;
            }
        }*/



        public async Task<decimal> CalculatePremiumAsync(PremiumCalculationDTO premiumDetails)
        {
            return await _premiumRL.CalculatePremiumAsync(premiumDetails);

        }

        public async Task<Policy> GetPolicyByIdAsync(int policyId)
        {
            try
            {
                var policy = await _premiumRL.GetPolicyByIdAsync(policyId);

                if (policy == null)
                {
                    throw new KeyNotFoundException($"Policy with ID {policyId} not found.");
                }

                return policy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Policy ID: {policyId}");
                throw;
            }
        }


    }
}

