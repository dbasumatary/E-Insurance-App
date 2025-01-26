using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.EmployeeDTO;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IAgentRL
    {
        Task<AgentEntity> RegisterAgentAsync(AgentEntity agent);
        Task<bool> IsEmailRegisteredAsync(string email);

        Task<AgentEntity> GetAgentByEmailAsync(string email);
        Task<AgentEntity> GetAgentByUsernameAsync(string username);

    }
}

