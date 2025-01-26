namespace eInsuranceApp.Entities.Dtos
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AgentID { get; set; }
        public AgentDTO Agent { get; set; }
    }
}
