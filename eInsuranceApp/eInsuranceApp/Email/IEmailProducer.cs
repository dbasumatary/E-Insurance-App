using eInsuranceApp.Entities.Email;

namespace eInsuranceApp.Email
{
    public interface IEmailProducer
    {
        //void SendEmailMessage(EmailMessage emailMessage);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
