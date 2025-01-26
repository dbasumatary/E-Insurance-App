using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Login;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class LoginRL : ILoginRL
    {
        private readonly AppDbContext _context;

        public LoginRL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IUser> GetUserByCredentialsAsync(string usernameOrEmail, string password)
        {
            var admin = await _context.Admin.FirstOrDefaultAsync(a =>
                (a.Username == usernameOrEmail || a.Email == usernameOrEmail) && a.Password == password);

            if (admin != null) return admin;

            var agent = await _context.Agents.FirstOrDefaultAsync(a =>
                (a.Username == usernameOrEmail || a.Email == usernameOrEmail) && a.Password == password);

            if (agent != null) return agent;

            var customer = await _context.Customers.FirstOrDefaultAsync(c =>
                (c.Email == usernameOrEmail) && c.Password == password);

            if (customer != null) return customer;

            var employee = await _context.Employees.FirstOrDefaultAsync(e =>
                (e.Username == usernameOrEmail || e.Email == usernameOrEmail) && e.Password == password);

            return employee;
        }


        public async Task<LoginEntity> Login(string emailOrUsername, string password)
        {
            try
            {
                var user = await _context.Admin
                    .FirstOrDefaultAsync(x => (x.Email.ToLower() == emailOrUsername.ToLower() || x.Username.ToLower() == emailOrUsername.ToLower()));
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        return new LoginEntity
                        {
                            EmailOrUsername = user.Email,
                            Role = "Admin"
                        };
                    }
                }

                var agent = await _context.Agents
                    .FirstOrDefaultAsync(x => (x.Email.ToLower() == emailOrUsername.ToLower() || x.Username.ToLower() == emailOrUsername.ToLower()));
                if (agent != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, agent.Password))
                    {
                        return new LoginEntity
                        {
                            EmailOrUsername = agent.Email,
                            Role = "Agent"
                        };
                    }
                }

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(x => (x.Email.ToLower() == emailOrUsername.ToLower()));
                if (customer != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, customer.Password))
                    {
                        return new LoginEntity
                        {
                            EmailOrUsername = customer.Email,
                            Role = "Customer"
                        };
                    }
                }

                var employee = await _context.Employees
                    .FirstOrDefaultAsync(x => (x.Email.ToLower() == emailOrUsername.ToLower() || x.Username.ToLower() == emailOrUsername.ToLower()));
                if (employee != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, employee.Password))
                    {
                        return new LoginEntity
                        {
                            EmailOrUsername = employee.Email,
                            Role = "Employee"
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during login.", ex);
            }
        }
    }
}
