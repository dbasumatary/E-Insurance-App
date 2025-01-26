using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Net.Mail;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using eInsuranceApp.Entities.Email;
using Microsoft.Extensions.Options;

namespace eInsuranceApp.Email
{
    public class EmailConsumer : IEmailConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EmailSettings _emailSettings;

        public EmailConsumer(IConnection connection, IOptions<EmailSettings> emailSettings)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _emailSettings = emailSettings.Value;

            // Declare the queue to ensure it exists before consuming
            _channel.QueueDeclare(queue: "emailQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                // Get the message from the queue
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                // Deserialize the email message
                var message = JsonConvert.DeserializeObject<EmailMessage>(messageJson);

                // Send the email
                await SendEmailAsync(message.To, message.Subject, message.Body);

                // Acknowledge the message after processing
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            // Start consuming from the queue
            _channel.BasicConsume(queue: "emailQueue", autoAck: false, consumer: consumer);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromAddress = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
            var toAddress = new MailAddress(to);
            var fromPassword = _emailSettings.FromPassword;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 30000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }

        
    }
}
