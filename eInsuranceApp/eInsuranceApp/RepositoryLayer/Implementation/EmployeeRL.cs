using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.EmployeeDTO;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.RepositoryLayer.Implementation
{
    public class EmployeeRL : IEmployeeRL
    {
        private readonly AppDbContext _context;

        public EmployeeRL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeEntity> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<EmployeeEntity> GetEmployeeByUsernameAsync(string username)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Username == username);
        }
    }
}
