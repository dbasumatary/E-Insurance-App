using eInsuranceApp.Entities.Payment;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Plans
{
    public class Scheme
    {
        [Key]
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string SchemeDetails { get; set; }
        public int PlanID { get; set; }  // Foreign key
        public decimal SchemeFactor { get; set; }
        public DateTime CreatedAt { get; set; }

        public InsurancePlan Plan { get; set; } 
        public ICollection<Policy> Policies { get; set; }
        public ICollection<PremiumCalculationDTO> Premiums { get; set; }

    }
}
