namespace eInsuranceApp.Entities.Payment
{
    public class PremiumEntity
    {
        public int CustomerID { get; set; }
        public int PolicyID { get; set; }
        public int SchemeID { get; set; }
        public decimal BaseRate { get; set; }
        
    }
}
