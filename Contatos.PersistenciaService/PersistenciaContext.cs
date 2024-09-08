using Microsoft.EntityFrameworkCore;
using ConsultaService.Models;

namespace ConsultaService
{
    public class PersistenciaContext : DbContext
    {
        public PersistenciaContext(DbContextOptions<PersistenciaContext> options) : base(options) { }

        public DbSet<Contato> Contatos { get; set; }
    }
}
