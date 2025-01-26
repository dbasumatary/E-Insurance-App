using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.Payment;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface ICommissionBL
    {
        //Task<List<CommissionEntity>> GetCommissionAsync(int agentId);
        Task<CommissionEntity> AddCommissionAsync(CommissionEntity commission);
        Task<AgentEntity> GetAgentByIdAsync(int agentId);
    }
}
