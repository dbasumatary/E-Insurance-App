using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class CommissionRL : ICommissionRL
    {
        public readonly AppDbContext _context;
        private readonly ILogger<CommissionRL> _logger;
        public CommissionRL(AppDbContext context, ILogger<CommissionRL> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CommissionEntity> AddCommissionAsync(CommissionEntity commission)
        {

            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC CalculateAgentCommission @AgentID",
                                                   new SqlParameter("@AgentID", commission.AgentID));

                _context.Commission.Add(commission);
                await _context.SaveChangesAsync();

                var createdCommission = await _context.Commission
                    .Include(c => c.Agent)
                    .Include(c => c.Premium)
                    .FirstOrDefaultAsync(c => c.CommissionID == commission.CommissionID);

                _logger.LogInformation("AddCommissionAsync completed successfully in Repository Layer.");
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
            _logger.LogInformation($"Fetching Agent details for AgentID: {agentId}");

            try
            {
                var agent = await _context.Agents.FirstOrDefaultAsync(a => a.AgentID == agentId);
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



        //async Task<List<CommissionEntity>> ICommissionRL.CalculateCommissionAsync(int agentId)
        //{
        //    if (agentId <= 0)
        //    {
        //        _logger.LogWarning("Invalid Agent ID provided in CalculateCommissionAsync.");
        //        throw new ArgumentException("Invalid Agent ID.");
        //    }

        //    try
        //    {
        //        var result = await _context.Commission
        //            .FromSqlInterpolated($"EXEC CalculateAgentCommission @AgentID = {agentId}")
        //            .ToListAsync();

        //        _logger.LogInformation($"Stored procedure is valid for AgentID: {agentId}");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred in getting commission for AgentID: {agentId}. Error: {ex.Message}");
        //        throw;
        //    }
        //}
    }
}
