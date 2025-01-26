using eInsuranceApp.Entities.Admin;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface IAdminRL
    {
        Task<int> AddAdminAsync(AdminEntity adminEntity);
    }
}
