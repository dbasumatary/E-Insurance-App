namespace eInsuranceApp.Entities.EmployeeDTO
{
    public class EmployeeRegistrationResponse
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
