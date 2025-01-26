namespace eInsuranceApp.Entities.CustomerDTO
{
    public class CustomerRegistrationRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int AgentID { get; set; }

    }
}

