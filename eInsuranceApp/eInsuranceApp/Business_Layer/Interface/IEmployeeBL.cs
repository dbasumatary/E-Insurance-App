using eInsuranceApp.Entities.EmployeeDTO;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IEmployeeBL
    {
        Task<EmployeeRegistrationResponse> RegisterEmployeeAsync(EmployeeRegistrationRequest employeeDTO);

    }
}
