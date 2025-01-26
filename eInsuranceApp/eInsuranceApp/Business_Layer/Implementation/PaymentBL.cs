using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.RepositoryLayer.Interface;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class PaymentBL : IPaymentBL
    {
        private readonly IPaymentRL _paymentRL;
        private readonly ILogger<PaymentBL> _logger;
        private readonly IPremiumRL _premiumRL;


        public PaymentBL(IPaymentRL paymentRL, ILogger<PaymentBL> logger, IPremiumRL premiumRL)
        {
            _paymentRL = paymentRL;
            _logger = logger;
            _premiumRL = premiumRL;
        }


        public async Task InsertPaymentsAsync(PaymentEntity payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");

            try
            {
                
                //var policy = await _paymentRL.GetPolicyByIdAsync(payment.PolicyID);
                //if (policy == null)
                //{
                //    throw new Exception($"Policy with ID {payment.PolicyID} is not found.");
                //}

                var premium = await _premiumRL.GetPremiumByIdAsync(payment.PremiumID);
                if (premium == null)
                {
                    throw new Exception($"Premium with ID {payment.PremiumID} is not found.");
                }

                int premiumID = premium.PremiumID;

                // Calculate
                if (payment.PaymentType == "Monthly")
                {
                    payment.Amount = premium.CalculatedPremium / 12;
                }
                else if (payment.PaymentType == "Yearly")
                {
                    payment.Amount = premium.CalculatedPremium;
                }
                else
                {
                    throw new Exception("Invalid PaymentType. It must be 'Monthly' or 'Yearly'.");
                }

                //payment.PremiumID = premiumID;

                await _paymentRL.InsertPaymentAsync(payment);
                await _paymentRL.UpdatePaymentStatusAsync(payment.PaymentID, "Processed");

                _logger.LogInformation($"Payment {payment.PaymentID} processed successfully at {DateTime.Now}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during the payment {payment.PaymentID}");
                throw;
            }
        }

        /*public async Task<PaymentEntity> GetPaymentsByIdAsync(int paymentId)
        {
            return await _paymentRL.GetPaymentByIdAsync(paymentId);
        }*/
        public async Task<PaymentEntity> GetPaymentsByIdAsync(int paymentId)
        {
            var paymentEntity = await _paymentRL.GetPaymentByIdAsync(paymentId);

            if (paymentEntity == null)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found.");
                return null;
            }

            return new PaymentEntity
            {
                PaymentID = paymentEntity.PaymentID,
                CustomerID = paymentEntity.CustomerID,
                PolicyID = paymentEntity.PolicyID,
                Amount = paymentEntity.Amount,
                PaymentType = paymentEntity.PaymentType,
                PaymentDate = paymentEntity.PaymentDate,
                Status = paymentEntity.Status,
                CreatedAt = paymentEntity.CreatedAt
            };
        }

        
    }
}
