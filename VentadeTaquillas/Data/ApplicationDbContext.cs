using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VentadeTaquillas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Administrador>
    {


        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Asiento> Asientos { get; set; }
        public DbSet<Taquilla> Taquillas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Cliente>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Administrador");
                entityTypeBuilder.Property(u => u.Nombre).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Apellido).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Cedula).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Usuario).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Telefono).HasMaxLength(100);

            });
        }

    }

        public class Cliente
        {
            public Guid ClienteId { get; set; }

            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Cedula { get; set; }
            public string Usuario { get; set; }
            public string Correo { get; set; }
            public string Ciudad { get; set; }
            public string Telefono { get; set; }
    }

        public class Administrador : IdentityUser
    {
            public Guid Id { get; set; }

            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Cedula { get; set; }
            public string Usuario { get; set; }
        public string Telefono { get; set; }
    }
        public class Asiento
        {
            public Guid AsientoId { get; set; }
            public int NumAsiento { get; set; }
        }

        public class Taquilla
        {
            public Guid Id { get; set; }
            public Guid ClienteId { get; set; }
            public Guid AsientoId { get; set; }
        }
}

