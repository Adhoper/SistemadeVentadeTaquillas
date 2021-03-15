using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VentadeTaquillas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Administrador>
    {

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Taquilla> Taquillas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Administrador>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Administrador");
                entityTypeBuilder.Property(u => u.Nombre).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Apellido).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Ciudad).HasMaxLength(100);

            });
        }

    }

        public class Administrador : IdentityUser
        {
            public Guid AdminId { get; set; }

            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Ciudad { get; set; }
        }


    public class Cliente
    {
        public Guid ClienteId { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
    }

        public class Taquilla
        {
            public Guid TaquillaId { get; set; }
            public Guid ClienteId { get; set; }
            public Guid AsientoId { get; set; }
        public Guid PeliculaId { get; set; }
        }

    public class Pelicula
    {
        public Guid PeliculaId { get; set; }
        public Guid NombrePeli { get; set; }
    }
}

