using Contatos.API.Interfaces;
using Contatos.API.Repositories;
using Contatos.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var stringConexao = configuration.GetConnectionString("DefaultConnection");       
        services.AddScoped<IDbConnection>((conexao) => new SqlConnection(stringConexao));
        services.AddScoped<IContatoRepository, ContatoRepository>();
        services.AddScoped<IContatoService, ContatoService>();
        services.AddScoped<IRegiaoRepository, RegiaoRepository>();
        services.AddScoped<IRegiaoService, RegiaoService>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddTransient<ILoggerService, LoggerService>();
        services.AddMemoryCache();
        services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}