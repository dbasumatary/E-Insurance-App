namespace eInsuranceApp.Entities.Plans
{
    public class PolicyCreateDTO
    {
        public int CustomerID { get; set; }
        public int SchemeID { get; set; }
        public string PolicyDetails { get; set; }
        //public decimal BasePremiumRate { get; set; }
        public DateTime DateIssued { get; set; }
        public int MaturityPeriod { get; set; }
        public DateTime PolicyLapseDate { get; set; }

    }
}

