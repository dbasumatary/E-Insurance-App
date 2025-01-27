using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Email;
using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Interface;
using eInsuranceApp.StaticClass;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class CustomerBL : ICustomerBL
    {
        private readonly ICustomerRL _customerRL;
        private readonly IEmailProducer _emailProducer;
        private readonly ILogger<EmployeeBL> _logger;

        public CustomerBL(ICustomerRL customerRL, IEmailProducer emailProducer, ILogger<EmployeeBL> logger)
        {
            _customerRL = customerRL;
            _emailProducer = emailProducer;
            _logger = logger;
        }

        public async Task<CustomerRegistrationResponse> RegisterCustomerAsync(CustomerRegistrationRequest customerDTO)
        {
            if (await _customerRL.IsEmailRegisteredAsync(customerDTO.Email))
                throw new ArgumentException("The email is already registered.");

            if (!await _customerRL.IsAgentExistsAsync(customerDTO.AgentID))
                throw new ArgumentException("The specified agent does not exist.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(customerDTO.Password);

            var customerEntity = new CustomerEntity
            {
                FullName = customerDTO.FullName,
                Email = customerDTO.Email,
                Phone = customerDTO.Phone,
                DateOfBirth = customerDTO.DateOfBirth,
                Password = passwordHash,
                Role = Entities.Login.UserRole.Customer,
                Username = customerDTO.Username,
                AgentID = customerDTO.AgentID
            };

            var savedCustomer = await _customerRL.RegisterCustomerAsync(customerEntity);

            // Send confirm
            var emailBody = $"Dear {savedCustomer.FullName},\n\nYour account has been successfully created.\nLogin Email: {savedCustomer.Email}\n";
            _emailProducer.SendEmailAsync(savedCustomer.Email, "Account Registration Successful", emailBody);

            _logger.LogInformation("Customer registration successful for {Email}.", savedCustomer.Email);

            return new CustomerRegistrationResponse
            {
                CustomerID = savedCustomer.CustomerID,
                FullName = savedCustomer.FullName,
                Email = savedCustomer.Email,
                Phone = savedCustomer.Phone,
                Role = savedCustomer.Role,
                DateOfBirth = savedCustomer.DateOfBirth,
                AgentID = savedCustomer.AgentID
            };
        }

        public async Task<int> GetCustomerAgeAsync(int customerId)
        {
            var customer = await _customerRL.GetCustomerByIdAsync(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer with ID {customerId} not found.");
            }

            var customerAge = customer.DateOfBirth.CalculateAge();
            return customerAge;
        }

        public async Task<List<PolicyViewDTO>> GetPoliciesForCustomerAsync(int customerId)
        {
            if (customerId <= 0)
            {
                _logger.LogWarning("Invalid CustomerID: {CustomerID}", customerId);
                throw new ArgumentException("Customer ID must be greater than zero.");
            }

            _logger.LogInformation("Fetching policies for CustomerID: {CustomerID}", customerId);
            return await _customerRL.GetPoliciesByCustomerIdAsync(customerId);
        }
    }
}
