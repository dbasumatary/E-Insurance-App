using eInsuranceApp.Business_Layer.Implementation;
using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class PolicyRL : IPolicyRL
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PolicyBL> _logger;

        public PolicyRL(AppDbContext dbContext, ILogger<PolicyBL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> ValidateAndCreatePolicyAsync(Policy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy), "Policy cannot be null.");

            try
            {
                // OUTPUT parameter for PolicyID
                var policyIdParam = new SqlParameter
                {
                    ParameterName = "@PolicyID",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                // Map
                var sqlParameters = new[]
                {
                    new SqlParameter("@CustomerID", policy.CustomerID),
                    new SqlParameter("@SchemeID", policy.SchemeID),
                    new SqlParameter("@PolicyDetails", policy.PolicyDetails ?? (object)DBNull.Value),
                    //new SqlParameter("@Premium", policy.Premium),
                    new SqlParameter("@DateIssued", policy.DateIssued),
                    new SqlParameter("@MaturityPeriod", policy.MaturityPeriod),
                    new SqlParameter("@PolicyLapseDate", policy.PolicyLapseDate),
                    policyIdParam
                };

                const string sqlCommand =
                    "EXEC ValidatePolicy @CustomerID, @SchemeID, @PolicyDetails, @DateIssued, @MaturityPeriod, @PolicyLapseDate, @PolicyID OUTPUT";

                // Execute sp
                await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand, sqlParameters);
                int policyId = (int)policyIdParam.Value;

                _logger?.LogInformation("Policy validated and created successfully for CustomerID: {CustomerID}, SchemeID: {SchemeID}",
                                         policy.CustomerID, policy.SchemeID);
                return policyId;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while validating and creating the policy for CustomerID: {CustomerID}",
                                  policy.CustomerID);
                throw;
            }

        }


        // to get a policy by ID
        public async Task<Policy> GetPolicyByIdAsync(int policyId)
        {
            try
            {
                var policy = await _dbContext.Policies
                    .Include(p => p.Customer) 
                    .Include(p => p.Scheme)
                    .FirstOrDefaultAsync(p => p.PolicyID == policyId);

                if (policy == null)
                {
                    _logger?.LogWarning($"Policy with ID {policyId} not found.");
                    return null; 
                }

                _logger?.LogInformation($"Fetched policy with ID {policyId} successfully.");
                return policy;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while fetching the policy.");
                throw;
            }
        }

        public async Task<decimal?> GetBasePremiumRateAsync(int schemeId)
        {
            try
            {
                var premium = await _dbContext.Premiums.FirstOrDefaultAsync(p => p.SchemeID == schemeId);
                return premium?.BaseRate;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while fetching the BaseRate for SchemeID: {SchemeID}", schemeId);
                throw;
            }
        }
    }
}

