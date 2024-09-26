using System.Text;
using System.Text.Json;
using Contatos.CadastroService.Dto;
using Prometheus;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var messageProducerCounter = Metrics.CreateCounter("rabbitmq_message_produced_total", "Total de mensagens enviadas para fila RabbitMQ.");

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var hostName = configuration["RabbitMQ:HostName"];
    var port = Convert.ToInt32(configuration["RabbitMQ:Port"]);
    var password = configuration["RabbitMQ:Password"];
    return new ConnectionFactory() { HostName = hostName, Port = port, Password = password };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseMetricServer();
app.UseHttpMetrics();
app.UseSerilogRequestLogging();

app.MapPost("/Contatos", (ContatoDtoRequest contato, IConnectionFactory factory) =>
{
    var hostName = configuration["RabbitMQ:HostName"];
    var port = Convert.ToInt32(configuration["RabbitMQ:Port"]);
    var password = configuration["RabbitMQ:Password"];    

    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "contatoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var message = JsonSerializer.Serialize(contato);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: "contatoQueue", basicProperties: null, body: body);
        messageProducerCounter.Inc();
    }

    return "Novo contato enviado para RabbitMQ";
})
.WithName("PostContato")
.WithOpenApi();

app.Run();

public partial class Program { }