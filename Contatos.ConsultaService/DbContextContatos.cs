using Microsoft.EntityFrameworkCore;
using ConsultaService.Models;

namespace ConsultaService
{
    public class DbContextContatos : DbContext
    {
        public DbContextContatos(DbContextOptions<DbContextContatos> options) : base(options) { }

        public DbSet<Contato> Contatos { get; set; }

        public DbSet<Regiao> Regioes { get; set; }
    }
}
