using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Contatos.AlteracaoService.Dto;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var messageProducerCounter = Metrics.CreateCounter("rabbitmq_message_produced_total", "Total de mensagens enviadas para fila RabbitMQ.");

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMetricServer();
app.UseHttpMetrics();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.MapPost("/AtualizaContato", (int id, ContatoDtoRequest contatoDto) => 
{
    var hostName = configuration["RabbitMQ:HostName"];
    var port = Convert.ToInt32(configuration["RabbitMQ:Port"]);
    var password = configuration["RabbitMQ:Password"];
    var factory = new ConnectionFactory() { HostName = hostName, Port = port, Password = password };

    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "alteracaoQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var contatoDtoResponse = new ContatoDtoResponse()
        {
            Id = id,
            DDD = contatoDto.DDD,
            Email = contatoDto.Email,
            Nome = contatoDto.Nome,
            Telefone = contatoDto.Telefone,
        };

        var message = JsonSerializer.Serialize(contatoDtoResponse);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: "alteracaoQueue", basicProperties: null, body: body);
        messageProducerCounter.Inc();
    }

    return "Nova alteracao de contato enviada para RabbitMQ";
})
.WithName("AtualizaContato")
.WithOpenApi();

app.Run();