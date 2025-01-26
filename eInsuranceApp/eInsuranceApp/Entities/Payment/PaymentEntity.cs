using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Payment
{
    public class PaymentEntity
    {
        [Key]
        public int PaymentID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int PolicyID { get; set; }
        [Required]
        public int PremiumID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public string Status { get; set; } = "Pending";
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public string PaymentType { get; set; }
        


        public CustomerEntity Customer { get; set; }
        public Policy Policy { get; set; }
        public PremiumCalculationDTO Premium { get; set; }
    }
}

