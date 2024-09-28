using Microsoft.EntityFrameworkCore;
using ConsultaService;
using Prometheus;
using Serilog;
using Contatos.PersistenciaService;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

builder.Services.AddDbContext<PersistenciaContext>(options => 
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<InclusaoConsumerService>();
builder.Services.AddHostedService<ExclusaoConsumerService>();
builder.Services.AddHostedService<AlteracaoConsumerService>();

var app = builder.Build();

app.UseMetricServer();
app.UseHttpMetrics();
app.UseSerilogRequestLogging();

app.Run();
