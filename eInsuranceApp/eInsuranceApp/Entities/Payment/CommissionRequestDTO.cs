using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Payment
{
    public class CommissionRequestDTO
    {
        [Required]
        public int AgentID { get; set; }

        [Required]
        public int PolicyID { get; set; }

        [Required]
        public int PremiumID { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
