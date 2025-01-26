using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.Login;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.CustomerDTO
{
    public class CustomerEntity : IUser
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int AgentID { get; set; }
        [Required]
        public string Role { get; set; } = UserRole.Customer;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public AgentEntity Agent { get; set; }
        public ICollection<Policy> Policies { get; set; }
        public ICollection<PaymentEntity> Payments { get; set; }
        public ICollection<PremiumCalculationDTO> PremiumCalculations { get; set; } 

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;

                return age;
            }
        }
    }
}

