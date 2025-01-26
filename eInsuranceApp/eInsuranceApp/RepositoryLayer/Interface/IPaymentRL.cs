using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IPaymentRL
    {
        Task InsertPaymentAsync(PaymentEntity payment);
        Task<PaymentEntity> GetPaymentByIdAsync(int paymentId);

        Task UpdatePaymentStatusAsync(int paymentId, string status);
        Task<Policy> GetPolicyByIdAsync(int policyId);

        Task<Policy?> GetPolicyAsync(int customerId, int policyId);

    }

}

