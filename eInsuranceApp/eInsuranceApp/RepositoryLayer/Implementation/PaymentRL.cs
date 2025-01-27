using eInsuranceApp.Business_Layer.Implementation;
using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class PaymentRL : IPaymentRL
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRL> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public PaymentRL(AppDbContext context, ILogger<PaymentRL> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
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


        public async Task<PaymentViewDTO> GetPaymentDetailsById(int paymentId)
        {
            string connectionString = _connectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("GetPaymentDetailsById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@PaymentID", paymentId);

                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var paymentDetail = new PaymentViewDTO
                    {
                        PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                        CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                        CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                        PolicyDetails = reader.GetString(reader.GetOrdinal("PolicyDetails")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                        PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        PaymentType = reader.GetString(reader.GetOrdinal("PaymentType")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };
                    return paymentDetail;
                }
                else
                {
                    _logger?.LogWarning("No Payment found for PaymentID: {PaymentID}", paymentId);
                    return null;
                }
                
            }
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

