using eInsuranceApp.Email;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace eInsuranceApp.EmailSend
{
    public class EmailSender
    {
        //public readonly IConnection _connection;

        private readonly IEmailProducer _emailProducer;

        public EmailSender(IEmailProducer emailProducer)
        {
            _emailProducer = emailProducer;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await _emailProducer.SendEmailAsync(to, subject, body);
        }

        /*public Task SendEmailAsync(string to, string subject, string body)
        {
            
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "emailQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var message = new
            {
                To = to,
                Subject = subject,
                Body = body
            };

            var bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish(exchange: "", routingKey: "emailQueue", basicProperties: null, body: bodyBytes);

            return Task.CompletedTask;
        }*/
    }
}
