using eInsuranceApp.Entities.Payment;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IPaymentBL
    {
        Task InsertPaymentsAsync(PaymentEntity payment);
        Task<PaymentEntity> GetPaymentsByIdAsync(int paymentId);
        Task<PaymentViewDTO> GetPaymentDetailsByIdAndLog(int paymentId);
    }
}
