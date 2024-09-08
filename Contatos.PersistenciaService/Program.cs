using Microsoft.EntityFrameworkCore;
using ConsultaService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<PersistenciaContext>(options => 
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<ContatoConsumerService>();

var app = builder.Build();

app.Run();
