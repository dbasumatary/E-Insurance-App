using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Login;
using eInsuranceApp.Entities.Payment;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.AgentDTO
{
    public class AgentEntity : IUser
    {
        [Key]
        public int AgentID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; } = UserRole.Agent;
        [Required]
        [Range(0, 1)]
        public decimal CommissionRate { get; set; } = 0.05M;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        

        public ICollection<CustomerEntity> Customers { get; set; }
        public ICollection<CommissionEntity> Commissions { get; set; }
    }
}

