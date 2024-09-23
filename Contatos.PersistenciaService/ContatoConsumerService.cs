using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ConsultaService.Models;
using System.Diagnostics.Metrics;
using Prometheus;
using Serilog;
using Serilog.Events;

namespace ConsultaService
{
    public class ContatoConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly Counter _messageConsumeCounter = Metrics.CreateCounter("rabbitmq_message_consumed_total", "Total de mensagens consumidas da fila RabbitMQ.");

        public ContatoConsumerService(IServiceProvider serviceProvider, IConfiguration configuration)
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

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
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

                            _messageConsumeCounter.Inc();
                        };

                        channel.BasicConsume(queue: "contatoQueue", autoAck: true, consumer: consumer);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(LogEventLevel.Error, $"Erro ao consumir mensagem da fila RabbitMQ: {ex.Message}");
                }
                
                await Task.Delay(1000, stoppingToken); // Aguarda por 1 segundo antes de verificar novamente
            }
        }
    }
}
