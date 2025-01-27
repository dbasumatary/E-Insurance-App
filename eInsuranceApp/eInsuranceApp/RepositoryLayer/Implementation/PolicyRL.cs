using eInsuranceApp.Business_Layer.Implementation;
using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class PolicyRL : IPolicyRL
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PolicyBL> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PolicyRL(AppDbContext dbContext, ILogger<PolicyBL> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> ValidateAndCreatePolicyAsync(Policy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy), "Policy cannot be null.");

            try
            {
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


        public async Task<PolicyViewDTO> GetPolicyDetailsByIdAsync(int policyId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetPolicyDetailsById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PolicyID", policyId));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var policyDetail = new PolicyViewDTO
                                {
                                    PolicyID = reader.GetInt32(reader.GetOrdinal("PolicyID")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                    SchemeName = reader.GetString(reader.GetOrdinal("SchemeName")),
                                    SchemeDetails = reader.GetString(reader.GetOrdinal("SchemeDetails")),
                                    PolicyDetails = reader.GetString(reader.GetOrdinal("PolicyDetails")),
                                    DateIssued = reader.GetDateTime(reader.GetOrdinal("DateIssued")),
                                    MaturityPeriod = reader.GetInt32(reader.GetOrdinal("MaturityPeriod")),
                                    PolicyLapseDate = reader.GetDateTime(reader.GetOrdinal("PolicyLapseDate")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                };

                                _logger?.LogInformation("Policy details found for PolicyID: {PolicyID}", policyId);
                                return policyDetail;
                            }
                            else
                            {
                                _logger?.LogWarning("No policy found for PolicyID: {PolicyID}", policyId);
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while fetching policy details for PolicyID: {PolicyID}", policyId);
                throw;
            }
        }


        

        public async Task<IEnumerable<PolicyViewDTO>> GetPoliciesAsync()
        {
            var policies = await _dbContext.Policies
                .Include(p => p.Customer)
                .Include(p => p.Scheme)
                .Select(p => new PolicyViewDTO
                {
                    PolicyID = p.PolicyID,
                    CustomerName = p.Customer.FullName,
                    CustomerEmail = p.Customer.Email,
                    SchemeName = p.Scheme.SchemeName,
                    SchemeDetails = p.Scheme.SchemeDetails,
                    PolicyDetails = p.PolicyDetails,
                    DateIssued = p.DateIssued,
                    MaturityPeriod = p.MaturityPeriod,
                    PolicyLapseDate = p.PolicyLapseDate,
                    Status = p.Status
                })
                .ToListAsync();

            return policies;
        }


        //By policyid or customername
        public async Task<PolicyViewDTO> SearchPolicyDetailsAsync(int? policyId, string customerName)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetPolicyDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.Add(new SqlParameter("@PolicyID", policyId ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@CustomerName", string.IsNullOrEmpty(customerName) ? (object)DBNull.Value : customerName));

                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var policyDetail = new PolicyViewDTO
                                {
                                    PolicyID = reader.GetInt32(reader.GetOrdinal("PolicyID")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                    SchemeName = reader.GetString(reader.GetOrdinal("SchemeName")),
                                    SchemeDetails = reader.GetString(reader.GetOrdinal("SchemeDetails")),
                                    PolicyDetails = reader.GetString(reader.GetOrdinal("PolicyDetails")),
                                    DateIssued = reader.GetDateTime(reader.GetOrdinal("DateIssued")),
                                    MaturityPeriod = reader.GetInt32(reader.GetOrdinal("MaturityPeriod")),
                                    PolicyLapseDate = reader.GetDateTime(reader.GetOrdinal("PolicyLapseDate")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                };

                                _logger?.LogInformation("Policy details found for PolicyID: {PolicyID} or CustomerName: {CustomerName}", policyId, customerName);
                                return policyDetail;
                            }
                            else
                            {
                                _logger?.LogWarning("No policy found for PolicyID: {PolicyID} or CustomerName: {CustomerName}", policyId, customerName);
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while fetching policy details for PolicyID: {PolicyID} or CustomerName: {CustomerName}", policyId, customerName);
                throw;
            }
        }




        //public async Task<Policy> GetPolicyDetailsByIdAsync(int policyId)
        //{
        //    try
        //    {
        //        //var policyIdParam = new SqlParameter("@PolicyID", policyId);

        //        var policyDetails = await _dbContext.Policies
        //            .FromSqlRaw("EXEC GetPolicyDetailsById @PolicyID", policyId)
        //            //.FromSqlInterpolated($"EXEC GetPolicyDetailsById @PolicyID = {policyId}")
        //            //.Include(p => p.Customer)
        //            //.Include(c => c.Scheme)
        //            .FirstOrDefaultAsync();

        //        if (policyDetails == null) {
        //            _logger.LogWarning($"No policy found for PolicyID: {policyId}");
        //            return null;
        //        }
        //        _logger?.LogInformation($"Policy details found for PolicyID: {policyId}");
        //        return policyDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, "Error occurred while fetching policy details for PolicyID: {PolicyID}", policyId);
        //        throw;
        //    }
        //}



        // to get a policy by ID
        //public async Task<Policy> GetPolicyByIdAsync(int policyId)
        //{
        //    try
        //    {
        //        var policy = await _dbContext.Policies
        //            .Include(p => p.Customer) 
        //            .Include(p => p.Scheme)
        //            .FirstOrDefaultAsync(p => p.PolicyID == policyId);

        //        if (policy == null)
        //        {
        //            _logger?.LogWarning($"Policy with ID {policyId} not found.");
        //            return null; 
        //        }

        //        _logger?.LogInformation($"Fetched policy with ID {policyId} successfully.");
        //        return policy;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, "Error occurred while fetching the policy.");
        //        throw;
        //    }
        //}

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

