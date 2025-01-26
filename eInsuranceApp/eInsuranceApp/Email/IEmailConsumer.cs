namespace eInsuranceApp.Email
{
    public interface IEmailConsumer
    {
        void StartConsuming();
        //Task SendEmailAsync(string to, string subject, string body);
    }
}
