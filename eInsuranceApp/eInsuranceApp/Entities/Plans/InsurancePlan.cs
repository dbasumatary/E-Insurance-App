namespace eInsuranceApp.Entities.Plans
{
    public class InsurancePlan
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string PlanDetails { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Scheme> Schemes { get; set; }
    }
}
