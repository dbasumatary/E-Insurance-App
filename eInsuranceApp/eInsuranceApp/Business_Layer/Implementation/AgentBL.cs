using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Email;
using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.Login;
using eInsuranceApp.RepositoryLayer.Interface;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class AgentBL : IAgentBL
    {
        private readonly IAgentRL _agentRL;
        private readonly IEmailProducer _emailProducer;
        //private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger<EmployeeBL> _logger;

        public AgentBL(IAgentRL agentrepo, IEmailProducer emailProducer, ILogger<EmployeeBL> logger)
        {
            _agentRL = agentrepo;
            _emailProducer = emailProducer;
            _logger = logger;
        }

        public async Task<AgentRegistrationResponse> RegisterAgentAsync(AgentRegistrationRequest agentDTO)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(agentDTO.Password);

            var agentEntity = new AgentEntity
            {
                Username = agentDTO.Username,
                Email = agentDTO.Email,
                FullName = agentDTO.FullName,
                Password = passwordHash,
                //Role = userRole,
                Role = UserRole.Agent,
                CommissionRate = agentDTO.CommissionRate,
            };

            // Save to the database
            var savedAgent = await _agentRL.RegisterAgentAsync(agentEntity);

            // Send confirmation email
            var emailBody = $"Welcome {savedAgent.FullName},\n\nYour username is {savedAgent.Username}.\nYour role is {savedAgent.Role}.\nYour login password is {agentDTO.Password}.\n";
            _emailProducer.SendEmailAsync(savedAgent.Email, "Agent Registration Successful", emailBody);

            // Log the event
            _logger.LogInformation($"Agent {savedAgent.Username} registered successfully with role {savedAgent.Role} at {DateTime.UtcNow}.");

            // Return response DTO
            return new AgentRegistrationResponse
            {
                AgentID = savedAgent.AgentID,
                Username = savedAgent.Username,
                Email = savedAgent.Email,
                FullName = savedAgent.FullName,
                Role = savedAgent.Role,
            };
        }

    }
}

