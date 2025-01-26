namespace eInsuranceApp.Business_Layer.Interface
{
    public interface ILoginBL
    {
        Task<string> Login(string emailOrUsername, string password);
    }
}
