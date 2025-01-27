namespace eInsuranceApp.Entities.Plans
{
    public class PolicySearchDTO
    {
        public int PolicyID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string SchemeName { get; set; }
        public string SchemeDetails { get; set; }
        public string PolicyStatus { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? PolicyLapseDate { get; set; }
        public string Status { get; set; }
    }
}
