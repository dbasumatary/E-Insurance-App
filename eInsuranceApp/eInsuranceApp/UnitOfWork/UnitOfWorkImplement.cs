using eInsuranceApp.DBContext;
using eInsuranceApp.RepositoryLayer.Implementation;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.UnitOfWork
{
    public class UnitOfWorkImplement : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IPolicyRL PolicyRepo { get; }
        public ICustomerRL CustomerRepo { get; }
        public IPremiumRL PremiumRepo { get; }


        public UnitOfWorkImplement(AppDbContext context, IPolicyRL policyRepo, ICustomerRL customerRepo, IPremiumRL premiumRepo)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            PolicyRepo = policyRepo ?? throw new ArgumentNullException(nameof(policyRepo));
            CustomerRepo = customerRepo ?? throw new ArgumentNullException(nameof(customerRepo));
            PremiumRepo = premiumRepo ?? throw new ArgumentNullException(nameof(premiumRepo)); ;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
