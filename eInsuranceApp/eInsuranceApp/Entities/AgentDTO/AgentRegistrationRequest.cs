namespace eInsuranceApp.Entities.AgentDTO
{
    public class AgentRegistrationRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Agent";
        public decimal CommissionRate { get; set; }
    }
}
