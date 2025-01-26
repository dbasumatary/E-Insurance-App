using eInsuranceApp.Entities.Login;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.EmployeeDTO
{
    public class EmployeeEntity : IUser
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; } = UserRole.Employee;

        //public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
