namespace eInsuranceApp.Entities.Dtos
{
    public class PolicyDTO
    {
        public int PolicyID { get; set; }
        public int CustomerID { get; set; }
        public int SchemeID { get; set; }
        public string PolicyDetails { get; set; }
        public decimal Premium { get; set; }
        public DateTime DateIssued { get; set; }
        public int MaturityPeriod { get; set; }
        public DateTime PolicyLapseDate { get; set; }
        public string Status { get; set; }

        public CustomerDTO Customer { get; set; }
        public SchemeDTO Scheme { get; set; }
    }
}
