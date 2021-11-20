using Microsoft.EntityFrameworkCore;
using Monedero.Models;

namespace Monedero.Context
{
    public class MonederoContext : DbContext
    {
        public MonederoContext(DbContextOptions<MonederoContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Movimiento> Movimientos { get; set; }

        public DbSet<Cuenta> Cuentas { get; set; }
    }
}
