using eInsuranceApp.Entities.Admin;

namespace eInsuranceApp.Business_Layer.Interface
{
    public interface IAdminBL
    {
        Task<AdminRegistrationResponse> RegisterAdminAsync(AdminRegistrationRequest request);

    }
}
