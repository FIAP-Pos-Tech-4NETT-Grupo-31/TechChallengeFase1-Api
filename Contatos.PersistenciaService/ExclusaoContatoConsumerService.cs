using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ConsultaService.Models;
using System.Diagnostics.Metrics;
using Prometheus;

namespace ConsultaService
{
    public class ExclusaoContatoConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly Counter _messageConsumeCounter = Metrics.CreateCounter("rabbitmq_message_consumed_total", "Total de mensagens consumidas da fila RabbitMQ.");

        public ExclusaoContatoConsumerService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var hostName = _configuration["RabbitMQ:HostName"];
            var port = Convert.ToInt32(_configuration["RabbitMQ:Port"]);
            var userName = _configuration["RabbitMQ:UserName"];
            var password = _configuration["RabbitMQ:Password"];
            var factory = new ConnectionFactory() { 
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password 
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "exclusaoContatoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var contatoId = JsonSerializer.Deserialize<ContatoId>(message);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<PersistenciaContext>();
                        var contato = await context.Contatos.FindAsync(contatoId?.Id);
                        if (contato != null)
                        {
                            context.Contatos.Remove(contato);
                            await context.SaveChangesAsync();
                        }    
                    }

                    _messageConsumeCounter.Inc();
                };

                channel.BasicConsume(queue: "exclusaoContatoQueue", autoAck: true, consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken); // Aguarda por 1 segundo antes de verificar novamente
                }
            }
        }
    }
}
