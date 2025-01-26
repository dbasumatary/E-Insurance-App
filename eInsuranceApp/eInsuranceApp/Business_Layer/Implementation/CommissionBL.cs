using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class CommissionBL : ICommissionBL
    {
        private readonly ICommissionRL _commissionRL;
        //private readonly IDistributedCache _cache;
        private readonly ILogger<CommissionBL> _logger;

        public  CommissionBL(ICommissionRL commissionRL, ILogger<CommissionBL> logger)
        {
            _commissionRL = commissionRL;
            //_cache = cache;
            _logger = logger;
        }

        public async Task<CommissionEntity> AddCommissionAsync(CommissionEntity commission)
        {

            try
            {
                if (commission.AgentID <= 0 || commission.PolicyID <= 0 || commission.PremiumID <= 0)
                {
                    _logger.LogWarning("Invalid data received in AddCommissionAsync.");
                    throw new ArgumentException("Invalid input data.");
                }


                var createdCommission = await _commissionRL.AddCommissionAsync(commission);

                _logger.LogInformation("AddCommissionAsync completed successfully.");
                return createdCommission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in AddCommissionAsync. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<AgentEntity> GetAgentByIdAsync(int agentId)
        {
            try
            {
                var agent = await _commissionRL.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    _logger.LogWarning($"Agent with ID {agentId} not found.");
                }
                return agent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching Agent details. Error: {ex.Message}");
                throw;
            }
        }



        //public async Task<List<CommissionEntity>> GetCommissionAsync(int agentId)
        //{

        //    if (agentId <= 0)
        //    {
        //        _logger.LogWarning("Invalid Agent ID provided.");
        //        throw new ArgumentException("Invalid Agent ID.");
        //    }

        //    string cacheKey = $"AgentCommission_{agentId}";

        //    try
        //    {
        //        // Cache
        //        var cachedData = await _cache.GetStringAsync(cacheKey);
        //        if (!string.IsNullOrEmpty(cachedData))
        //        {
        //            _logger.LogInformation($"Cache found for AgentID: {agentId}");
        //            return JsonConvert.DeserializeObject<List<CommissionEntity>>(cachedData);
        //        }

        //        // DB
        //        _logger.LogInformation($"Cache not found for AgentID: {agentId}, so will fetch from database.");
        //        var commissions = await _commissionRL.CalculateCommissionAsync(agentId);

        //        // Store in Cache
        //        if (commissions != null && commissions.Count > 0)
        //        {
        //            var cacheOptions = new DistributedCacheEntryOptions
        //            {
        //                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        //            };
        //            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(commissions), cacheOptions);
        //            _logger.LogInformation($"Cache updated for AgentID: {agentId}");

        //        }

        //        return commissions;
        //    }
        //    catch (Exception ex) 
        //    {
        //        _logger.LogError($"Error occurred in getting commission for AgentID: {agentId}. Error: {ex.Message}");
        //        throw;
        //    }

        //}
    }
}
