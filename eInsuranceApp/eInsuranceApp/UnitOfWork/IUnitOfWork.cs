using eInsuranceApp.RepositoryLayer.Interface;

namespace eInsuranceApp.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPolicyRL PolicyRepo{ get; }
        ICustomerRL CustomerRepo { get; }
        Task<int> SaveChangesAsync();
        IPremiumRL PremiumRepo { get; }
        
    }
}
