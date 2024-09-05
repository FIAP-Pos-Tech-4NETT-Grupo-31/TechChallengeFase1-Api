using Microsoft.EntityFrameworkCore;
using PersistenciaService.Models;

namespace PersistenciaService
{
    public class PersistenciaContext : DbContext
    {
        public PersistenciaContext(DbContextOptions<PersistenciaContext> options) : base(options) { }

        public DbSet<Contato> Contatos { get; set; }
    }
}
