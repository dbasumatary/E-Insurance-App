namespace eInsuranceApp.Entities.Plans
{
    public class PolicyViewDTO
    {
        public int PolicyID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string SchemeName { get; set; }
        public string SchemeDetails { get; set; }
        //public decimal PremiumAmount { get; set; }
        public string PolicyDetails { get; set; }
        public DateTime DateIssued { get; set; }
        public int MaturityPeriod { get; set; }
        public DateTime PolicyLapseDate { get; set; }
        public string Status { get; set; }
    }
}
