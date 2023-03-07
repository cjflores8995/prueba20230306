using Dominio.Enumeradores;
using Dominio.Modelos;

using Microsoft.EntityFrameworkCore;

using System;

namespace Persistence
{
    public class BcoDbContext : DbContext
    {
        public BcoDbContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Persona> Persona { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Movimiento> Movimiento { get; set; }
    }
}
