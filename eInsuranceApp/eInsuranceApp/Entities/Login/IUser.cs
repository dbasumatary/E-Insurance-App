namespace eInsuranceApp.Entities.Login
{
    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        string FullName { get; set; }
        string Role { get; set; }
    }
}

