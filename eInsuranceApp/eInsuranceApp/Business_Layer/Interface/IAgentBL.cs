using eInsuranceApp.Entities.AgentDTO;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IAgentBL
    {
        Task<AgentRegistrationResponse> RegisterAgentAsync(AgentRegistrationRequest agentDTO);
    }
}
