namespace eInsuranceApp.Entities.CustomerDTO
{
    public class CustomerRegistrationResponse
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AgentID { get; set; }

    }
}

