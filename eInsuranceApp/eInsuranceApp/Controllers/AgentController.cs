using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.AgentDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentBL _agentBL;

        public AgentController(IAgentBL agentBL)
        {
            _agentBL = agentBL;
        }

        [HttpPost("register")]
        //[Authorize]
        public async Task<IActionResult> RegisterAgent([FromBody] AgentRegistrationRequest agentdto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _agentBL.RegisterAgentAsync(agentdto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
