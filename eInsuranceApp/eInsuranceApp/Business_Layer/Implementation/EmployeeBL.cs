using Azure.Core;
using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Email;
using eInsuranceApp.Entities.EmployeeDTO;
using eInsuranceApp.RepositoryLayer.Implementation;
using eInsuranceApp.RepositoryLayer.Interface;
using eInsuranceApp.RepositoryLayer.Service;
using Microsoft.Extensions.Logging;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeRL _employeeRepository;
        private readonly IEmailProducer _emailProducer;
        //private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger<EmployeeBL> _logger;

        public EmployeeBL(IEmployeeRL employeeRepository, IEmailProducer emailProducer, ILogger<EmployeeBL> logger)
        {
            _employeeRepository = employeeRepository;
            _emailProducer = emailProducer;
            _logger = logger;
        }

        

        public async Task<EmployeeRegistrationResponse> RegisterEmployeeAsync(EmployeeRegistrationRequest employeeDTO)
        {
            
            if (await _employeeRepository.GetEmployeeByEmailAsync(employeeDTO.Email) != null)
                throw new Exception("Email is already registered.");

            if (await _employeeRepository.GetEmployeeByUsernameAsync(employeeDTO.Username) != null)
                throw new Exception("Username is already taken.");

            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString().Substring(0, 8));
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeDTO.Password);

            var employee = new EmployeeEntity
            {
                FullName = employeeDTO.FullName,
                Email = employeeDTO.Email,
                Username = employeeDTO.Username,
                Password = hashedPassword,
                Role = Entities.Login.UserRole.Employee
            };

            employee = await _employeeRepository.AddEmployeeAsync(employee);

            _logger.LogInformation($"Employee Registered: {employeeDTO.Username}, ID: {employee}");
            // Send confirm
            string message = $"Welcome {employee.FullName},\nYour account has been created. Username: {employee.Username}, Password: {hashedPassword}";
            await _emailProducer.SendEmailAsync(employee.Email, "Employee Account Created", message);

            return new EmployeeRegistrationResponse
            {
                EmployeeID = employee.EmployeeID,
                FullName = employee.FullName,
                Email = employee.Email,
                Username = employee.Username,
                Role = employee.Role,
                CreatedAt = employee.CreatedAt
            };
        }
    }
}
