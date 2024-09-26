using System.Text;
using System.Text.Json;
using Contatos.ExclusaoService.Dto;
using Microsoft.AspNetCore.Mvc;
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

app.MapDelete("/ExclusaoContato", ([FromBody] ExclusaoContatoDtoRequest contato) =>
{
    var hostName = configuration["RabbitMQ:HostName"];
    var port = Convert.ToInt32(configuration["RabbitMQ:Port"]);
    var password = configuration["RabbitMQ:Password"];
    var factory = new ConnectionFactory() { HostName = hostName, Port = port, Password = password };

    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "exclusaoContatoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var message = JsonSerializer.Serialize(contato);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: "exclusaoContatoQueue", basicProperties: null, body: body);
        messageProducerCounter.Inc();
    }

    return "Nova exclusão de contato enviada para RabbitMQ";
})
.WithName("DeleteContato")
.WithOpenApi();

app.Run();

