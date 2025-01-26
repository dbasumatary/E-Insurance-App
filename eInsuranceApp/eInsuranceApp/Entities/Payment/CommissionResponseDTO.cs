namespace eInsuranceApp.Entities.Payment
{
    public class CommissionResponseDTO
    {
        public int CommissionID { get; set; }
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        public int PolicyID { get; set; }
        public int PremiumID { get; set; }
        public decimal CommissionAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
