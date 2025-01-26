using eInsuranceApp.Entities.Login;

namespace eInsuranceApp.RepositoryLayer.Interface
{
    public interface ILoginRL
    {
        Task<LoginEntity> Login(string emailOrUsername, string password);
        Task<IUser> GetUserByCredentialsAsync(string usernameOrEmail, string password);
    }
}
