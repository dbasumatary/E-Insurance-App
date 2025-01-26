using eInsuranceApp.Entities.Email;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eInsuranceApp.Email
{
    public class EmailProducer : IEmailProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EmailProducer(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();

            // Declare a queue to ensure that the email queue exists
            _channel.QueueDeclare(queue: "emailQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new
            {
                To = to,
                Subject = subject,
                Body = body
            };

            // Serialize message into a byte array
            var bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            // Publish the message to the queue
            _channel.BasicPublish(
                exchange: "",
                routingKey: "emailQueue",
                basicProperties: null,
                body: bodyBytes
            );

            return Task.CompletedTask;
        }






        //private readonly IConfiguration _configuration;

        //public EmailProducer(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}




        /*public void SendEmailMessage(EmailMessage emailMessage)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQSettings:HostName"],
                UserName = _configuration["RabbitMQSettings:UserName"],
                Password = _configuration["RabbitMQSettings:Password"]
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = _configuration["RabbitMQSettings:QueueName"];
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonSerializer.Serialize(emailMessage);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($" [x] Sent message to queue: {message}");
            }
        }*/


    }
}
