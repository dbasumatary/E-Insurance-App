using eInsuranceApp.Business_Layer.Implementation;
using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using eInsuranceApp.StaticClass;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class PremiumRL : IPremiumRL
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogger<PaymentRL> _logger;

        public PremiumRL(AppDbContext dbContext, IConfiguration configuration, ILogger<PaymentRL> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<decimal> CalculatePremiumAsync(PremiumCalculationDTO premiumDetails)
        {
            var customer = await _dbContext.Customers
                                   .FirstOrDefaultAsync(c => c.CustomerID == premiumDetails.CustomerID);
            
                if (customer == null)
            {
                throw new Exception($"Customer with ID {premiumDetails.CustomerID} not found.");
            }

            

            var customerAge = CalculateAge(customer.DateOfBirth);

            var customerIdParam = new SqlParameter("@CustomerID", premiumDetails.CustomerID);
            var schemeIdParam = new SqlParameter("@SchemeID", premiumDetails.SchemeID);
            var policyIdParam = new SqlParameter("@PolicyID", premiumDetails.PolicyID);
            var ageParam = new SqlParameter("@CustomerAge", customerAge);
            var baseRateParam = new SqlParameter("@BaseRate", premiumDetails.BaseRate);
            var createdAt = new SqlParameter("@CreatedAt", premiumDetails.CreatedAt);
            var calculatedPremiumParam = new SqlParameter("@CalculatedPremium", SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };
            //decimal calculatedPremium = (decimal)calculatedPremiumParam.Value;

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC CalculatePremium @CustomerID, @PolicyID, @SchemeID, @BaseRate, @CustomerAge, @CalculatedPremium OUTPUT",
                customerIdParam, policyIdParam, schemeIdParam, baseRateParam, ageParam, calculatedPremiumParam
            );
            decimal calculatedPremium = (decimal)(calculatedPremiumParam.Value ?? 0);

            //decimal calculatedPremium = calculatedPremiumParam.Value != DBNull.Value
            //? (decimal)calculatedPremiumParam.Value
            //: 0;

            _logger.LogInformation($"Calculated Premium for CustomerID {premiumDetails.CustomerID}: {calculatedPremium}");


            var premiumdto = new PremiumCalculationDTO
            {
                CustomerID = premiumDetails.CustomerID,
                PolicyID = premiumDetails.PolicyID,
                SchemeID = premiumDetails.SchemeID,
                BaseRate = premiumDetails.BaseRate,
                Age = customerAge,
                CalculatedPremium = calculatedPremium,
                CreatedAt = premiumDetails.CreatedAt
            };

            await _dbContext.Premiums.AddAsync(premiumdto);
            await _dbContext.SaveChangesAsync();
            return calculatedPremium;
        }


        public async Task<PremiumCalculationDTO> GetPremiumRateAsync(int schemeId)
        {
            try
            {
                var premium = await _dbContext.Premiums
                    .FirstOrDefaultAsync(p => p.SchemeID == schemeId);

                if (premium == null)
                {
                    _logger.LogWarning($"No premium rate found for SchemeID: {schemeId}");
                    return null;
                }

                return premium;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching the premium rate.");
                throw;
            }
        }

        public async Task<Policy> GetPolicyByIdAsync(int policyId)
        {
            try
            {
                var policy = await _dbContext.Policies
                    .FirstOrDefaultAsync(p => p.PolicyID == policyId);

                return policy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Policy ID: {policyId}");
                throw;
            }
        }

        public async Task<PremiumCalculationDTO> GetPremiumByIdAsync(int premiumId)
        {
            if (premiumId <= 0)
                throw new ArgumentException("Invalid PremiumID provided.", nameof(premiumId));

            try
            {
                var prem = await _dbContext.Premiums.FirstOrDefaultAsync(p => p.PremiumID == premiumId);
                return prem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Premium ID: {premiumId}");
                throw;
            }
        }

        public async Task<PremiumCalculationDTO> GetPremiumBySchemeIdAsync(int schemeId)
        {
            if (schemeId <= 0)
                throw new ArgumentException("Invalid SchemeID provided.", nameof(schemeId));

            try
            {
                _logger.LogInformation($"Fetching premium for SchemeID {schemeId}.");

                var premium = await _dbContext.Set<PremiumCalculationDTO>()
                    .FirstOrDefaultAsync(p => p.SchemeID == schemeId);

                if (premium == null)
                {
                    _logger.LogWarning($"No premium found for SchemeID {schemeId}.");
                    return null;
                }

                _logger.LogInformation($"Successfully retrieved premium for SchemeID {schemeId}.");
                return premium;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the premium for SchemeID {schemeId}.");
                throw;
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            return age;
        }

        /*public async Task<decimal> CalculatePremiumAsync(int customerId, int policyId, int schemeId, decimal baseRate, int customerAge)
        {
            try
            {
                var sqlParameters = new[]
                {
                    new SqlParameter("@CustomerID", customerId),
                    new SqlParameter("@PolicyID", policyId),
                    new SqlParameter("@SchemeID", schemeId),
                    new SqlParameter("@BaseRate", baseRate),
                    new SqlParameter("@CustomerAge", customerAge),
                    new SqlParameter("@CalculatedPremium", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 10, Scale = 2 }
                };

                const string sqlcommand = "EXEC CalculatePremium @CustomerID, @PolicyID, @SchemeID, @BaseRate, @CustomerAge, @CalculatedPremium OUTPUT";
                await _dbContext.Database.ExecuteSqlRawAsync(sqlcommand, sqlParameters);

                decimal calculatedPremium = (decimal)sqlParameters[5].Value;
                return calculatedPremium;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating premium.");
                throw;
            }
        }*/



        /*public async Task<decimal> CalculatePremiumAsync(PremiumCalculationDTO premiumDetails)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("CalculatePremium", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@CustomerID", premiumDetails.CustomerID);
                command.Parameters.AddWithValue("@SchemeID", premiumDetails.SchemeID);
                command.Parameters.AddWithValue("@Age", premiumDetails.Age);
                command.Parameters.AddWithValue("@BaseRate", premiumDetails.BaseRate);
                var outputParam = new SqlParameter("@CalculatedPremium", SqlDbType.Decimal)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                await command.ExecuteNonQueryAsync();

                return (decimal)outputParam.Value;
            }
        }*/

    }
}
