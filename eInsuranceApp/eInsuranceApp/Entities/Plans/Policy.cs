using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Payment;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Plans
{
    public class Policy
    {
        [Key]
        public int PolicyID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int SchemeID { get; set; } 
        public string PolicyDetails { get; set; }
        [Required]
        //public decimal BasePremiumRate { get; set; }

        //public decimal? Premium { get; set; }
        public DateTime DateIssued { get; set; }
        public int MaturityPeriod { get; set; }
        public DateTime PolicyLapseDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Pending";

        public CustomerEntity Customer { get; set; } 
        public Scheme Scheme { get; set; }

        public ICollection<PaymentEntity> Payments { get; set; }
        public ICollection<PremiumCalculationDTO> PremiumCalculations { get; set; }

    }
}
