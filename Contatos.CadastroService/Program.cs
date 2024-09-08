using System.Text;
using System.Text.Json;
using Contatos.CadastroService.Interfaces;
using Contatos.CadastroService.Dto;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/contato", (ContatoDtoRequest contato) =>
{
    var hostName = configuration["RabbitMQ:HostName"];
    var port = Convert.ToInt32(configuration["RabbitMQ:Port"]);
    var password = configuration["RabbitMQ:Password"];
    var factory = new ConnectionFactory() { HostName = hostName, Port = port, Password = password };

    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "contatoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var message = JsonSerializer.Serialize(contato);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: "contatoQueue", basicProperties: null, body: body);
    }

    return "Novo contato enviado para RabbitMQ";
})
.WithName("PostContato")
.WithOpenApi();

app.Run();

