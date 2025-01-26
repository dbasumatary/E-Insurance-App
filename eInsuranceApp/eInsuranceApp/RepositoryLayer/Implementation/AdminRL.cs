using eInsuranceApp.DBContext;
using eInsuranceApp.Entities.Admin;
using eInsuranceApp.RepositoryLayer.Interface;

namespace eInsuranceApp.RepositoryLayer.Service
{
    public class AdminRL : IAdminRL
    {
        public readonly AppDbContext _context;

        public AdminRL(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddAdminAsync(AdminEntity adminEntity)
        {
            _context.Admin.Add(adminEntity);
            await _context.SaveChangesAsync();
            return adminEntity.AdminID;
        }
    }
}
