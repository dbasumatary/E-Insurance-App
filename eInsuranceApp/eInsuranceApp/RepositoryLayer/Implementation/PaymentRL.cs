using eInsuranceApp.Business_Layer.Implementation;
using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class PaymentRL : IPaymentRL
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRL> _logger;
        public PaymentRL(AppDbContext context, ILogger<PaymentRL> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InsertPaymentAsync(PaymentEntity payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Policy cannot be null.");

            try
            {
                // Map
                var sqlParameters = new[]
                {
                    new SqlParameter("@CustomerID", payment.CustomerID),
                    new SqlParameter("@PolicyID", payment.PolicyID),
                    new SqlParameter("@Amount", payment.Amount),
                    new SqlParameter("@PaymentType", payment.PaymentType),
                    new SqlParameter("@PaymentDate", payment.PaymentDate),
                    new SqlParameter("@Status", payment.Status),
                    new SqlParameter("@CreatedAt", payment.CreatedAt),
                    new SqlParameter("@PremiumID", payment.PremiumID)
                };

                const string sqlCommand =
                    "EXEC ProcessPayment @CustomerID, @PolicyID, @Amount, @PaymentType, @PaymentDate, @PremiumID";

                // Execute sp
                await _context.Database.ExecuteSqlRawAsync(sqlCommand, sqlParameters);

                _logger?.LogInformation($"Payment created successfully for CustomerID: {payment.CustomerID}, PolicyID: {payment.PolicyID}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error occurred while payment for CustomerID: {payment.CustomerID}");
                throw;
            }

            //await _context.Payments.AddAsync(payment);
            //await _context.SaveChangesAsync();
        }

        public async Task<PaymentEntity> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentID == paymentId);
        }

        public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var updatePay = await _context.Payments.FindAsync(paymentId);
            if (updatePay != null) { 
                updatePay.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Policy> GetPolicyByIdAsync(int policyId)
        {
            return await _context.Policies.FirstOrDefaultAsync(p => p.PolicyID == policyId);
        }

        public async Task<Policy?> GetPolicyAsync(int customerId, int policyId)
        {
            return await _context.Policies
                .FirstOrDefaultAsync(p => p.CustomerID == customerId && p.PolicyID == policyId);
        }

        
    }
}

