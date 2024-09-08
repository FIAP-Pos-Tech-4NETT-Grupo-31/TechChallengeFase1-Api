using ConsultaService;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddDbContext<DbContextContatos>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/Contatos", async (DbContextContatos dbContext) =>
{
    var contatos = await dbContext.Contatos.ToListAsync();
    return Results.Ok(contatos);
})
.WithName("GetContatos")
.WithOpenApi();

app.MapGet("/Contatos/{id}", async (DbContextContatos dbContext, int id) =>
{
    var contato = await dbContext.Contatos.FindAsync(id);
    if (contato == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(contato);
})
.WithName("GetContato")
.WithOpenApi();

app.MapGet("/Regioes", async (DbContextContatos dbContext) =>
{
    var regioes = await dbContext.Regioes.ToListAsync();
    return Results.Ok(regioes);
});

app.Run();
