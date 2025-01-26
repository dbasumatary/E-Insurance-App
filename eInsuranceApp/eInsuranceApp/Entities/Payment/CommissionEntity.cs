using eInsuranceApp.Entities.AgentDTO;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Payment
{
    public class CommissionEntity
    {
        [Key]
        public int CommissionID { get; set; }
        [Required]
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        [Required]
        public int PolicyID { get; set; }
        [Required]
        public int PremiumID { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CommissionAmount { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public AgentEntity Agent { get; set; }
        public PremiumCalculationDTO Premium { get; set; }
    }
}

