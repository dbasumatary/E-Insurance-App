using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Email;
using eInsuranceApp.EmailSend;
using eInsuranceApp.Entities.Admin;
using eInsuranceApp.RepositoryLayer.Interface;
using eInsuranceApp.RepositoryLayer.Service;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class AdminBL : IAdminBL
    {
        public readonly IAdminRL _adminRL;
        private readonly ILogger<AdminBL> _logger;
        private readonly IEmailProducer _emailProducer;

        public AdminBL(IAdminRL adminRL, ILogger<AdminBL> logger, IEmailProducer emailProducer)
        {
            _adminRL = adminRL;
            _logger = logger;
            _emailProducer = emailProducer;
        }

        public async Task<AdminRegistrationResponse> RegisterAdminAsync(AdminRegistrationRequest request)
        {
            // Hash
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var admin = new AdminEntity
            {
                Username = request.Username,
                Password = hashedPassword,
                Email = request.Email,
                FullName = request.FullName,
                Role = Entities.Login.UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            // Database
            var adminId = await _adminRL.AddAdminAsync(admin);

            // Send Confirm mail
            //var message = $"Welcome {request.FullName},\nYour account has been created.\n Username: {request.Username},\nAdminId: {adminId}";
            //await _emailSender.SendEmailAsync(request.Email, "Admin Account Created", message);

            var emailSubject = "Welcome to the E-Insurance Platform";
            var emailBody = $"Hello {request.FullName},\n\n" +
                            $"Your admin account has been created successfully. Here are your login details:\n\n" +
                            $"Username: {request.Username}\n" +
                            $"Admin ID: {adminId}\n\n" +
                            $"Thank you for being a part of the E-Insurance team!";

            await _emailProducer.SendEmailAsync(request.Email, emailSubject, emailBody);

            // Loggin
            _logger.LogInformation($"Admin Registered: {request.Username}, ID: {adminId}");

            return new AdminRegistrationResponse
            {
                AdminID = adminId,
                Username = request.Username,
                Email = request.Email,
                Message = "Admin registered successfully."
            };
        }
    }
}
