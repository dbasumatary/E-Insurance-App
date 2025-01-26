using eInsuranceApp.Entities.Login;
using System.ComponentModel.DataAnnotations;

namespace eInsuranceApp.Entities.Admin
{
    public class AdminEntity : IUser
    {
        [Key]
        public int AdminID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Role { get; set; } = UserRole.Admin;

        public DateTime CreatedAt { get; set; }
    }
}
