using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using PersistenciaService.Models;

namespace PersistenciaService
{
    public class ContatoConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ContatoConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, Password = "jNw!4WSV2RRhmTz" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "contatoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var contato = JsonSerializer.Deserialize<Contato>(message);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<PersistenciaContext>();
                        context.Contatos.Add(contato);
                        await context.SaveChangesAsync();
                    }
                };

                channel.BasicConsume(queue: "contatoQueue", autoAck: true, consumer: consumer);

                await Task.CompletedTask;
            }
        }
    }
}
