using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.Payment;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface ICommissionRL
    {
        //Task<CommissionEntity> CalculateCommissionAsync(int agentId);
        //Task<List<CommissionEntity>> CalculateCommissionAsync(int agentId);
        Task<CommissionEntity> AddCommissionAsync(CommissionEntity commission);
        Task<AgentEntity> GetAgentByIdAsync(int agentId);
        Task<CommissionViewDTO> GetCommissionDetailsById(int commissionId);
    }
}
