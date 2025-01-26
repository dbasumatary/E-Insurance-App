using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Payment
{
    public class PremiumCalculationDTO
    {
        [Key]
        public int PremiumID { get; set; }
        public int CustomerID { get; set; }
        public int PolicyID { get; set; }
        public int SchemeID { get; set; }
        public decimal BaseRate { get; set; }
        public int Age { get; set; }
        public decimal CalculatedPremium { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CustomerEntity Customer { get; set; }
        public Policy Policy { get; set; }
        public Scheme Scheme { get; set; }

        public ICollection<PaymentEntity> Payments { get; set; }
    }
}
