using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class AgentRL : IAgentRL
    {
        private readonly AppDbContext _context;

        public AgentRL(AppDbContext context)
        {
            _context = context;
        }


        public async Task<AgentEntity> RegisterAgentAsync(AgentEntity agent)
        {
            _context.Agents.Add(agent);

            await _context.SaveChangesAsync();

            return agent;
        }


        public async Task<AgentEntity> GetAgentByEmailAsync(string email)
        {
            return await _context.Agents.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<AgentEntity> GetAgentByUsernameAsync(string username)
        {
            return await _context.Agents.FirstOrDefaultAsync(x => x.Username == username);
        }
        

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _context.Agents.AnyAsync(a => a.Email == email);

        }


    }
}
