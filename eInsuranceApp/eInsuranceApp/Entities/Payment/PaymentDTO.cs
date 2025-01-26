using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Payment
{
    public class PaymentDTO
    {
        
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int PolicyID { get; set; }
        [Required]
        public int PremiumID { get; set; }
        [Required]
        public string PaymentType { get; set; }
        //public decimal Amount { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        //public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        

    }
}
