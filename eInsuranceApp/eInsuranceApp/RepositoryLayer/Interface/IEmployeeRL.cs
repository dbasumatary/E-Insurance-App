using eInsuranceApp.Entities.EmployeeDTO;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IEmployeeRL
    {
        Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity employee);
        Task<EmployeeEntity> GetEmployeeByEmailAsync(string email);
        Task<EmployeeEntity> GetEmployeeByUsernameAsync(string username);

    }
}
