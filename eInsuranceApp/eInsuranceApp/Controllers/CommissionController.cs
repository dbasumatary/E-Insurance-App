using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionBL _commissionBL;
        private readonly ILogger<CommissionController> _logger;

        public CommissionController(ICommissionBL commissionBL, ILogger<CommissionController> logger)
        {
            _commissionBL = commissionBL;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCommission(CommissionRequestDTO commissionRequest)
        {
            try
            {
                //var commissions = await _commissionBL.GetCommissionAsync(agentId);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid request received.");
                    return BadRequest(ModelState);
                }

                var agent = await _commissionBL.GetAgentByIdAsync(commissionRequest.AgentID);
                if (agent == null)
                {
                    _logger.LogWarning($"Agent with ID {commissionRequest.AgentID} not found.");
                    return NotFound($"Agent with ID {commissionRequest.AgentID} not found.");
                }

                var commissionEntity = new CommissionEntity
                {
                    AgentID = commissionRequest.AgentID,
                    PolicyID = commissionRequest.PolicyID,
                    PremiumID = commissionRequest.PremiumID,
                    CreatedAt = commissionRequest.CreatedAt,
                    AgentName = agent.FullName
                };

                var createdCommission = await _commissionBL.AddCommissionAsync(commissionEntity);

                var responseDTO = new CommissionResponseDTO
                {
                    CommissionID = createdCommission.CommissionID,
                    AgentID = createdCommission.AgentID,
                    AgentName = createdCommission.AgentName,
                    PolicyID = createdCommission.PolicyID,
                    PremiumID = createdCommission.PremiumID,
                    CommissionAmount = createdCommission.CommissionAmount,
                    CreatedAt = createdCommission.CreatedAt
                };

                _logger.LogInformation("Commission successfully added.");
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in addinf commission. Error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}


