using System;
using System.Collections.Generic;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using System.IO;  

namespace AppClientesMexaba.Models  
{
    public class ClContext : BaseDbContext
    {
        public ClContext(DbContextOptions<ClContext> options) : base(options)
        {
        }

    }

    public class BaseDbContext : DbContext
    {
        //USUARIOS Y CATALOGOS
        public DbSet<tcausr> Tcausr { get; set; } // USUARIOS MERKSYST
        public DbSet<ccecpo> CodigosPostales { get; set; } // CATALOGO CODIGOS POSTALES
        public DbSet<comestados> Comestados { get; set; } // CATALOGO CIUDADES

        //CLIENTES
        public DbSet<cxccli> Cxccli { get; set; } //  CXCCLI
        public DbSet<cxccli_ad> Cxccli_ad { get; set; } //CXCCLI_AD
        public DbSet<cxccli_sat> Cxccli_sat { get; set; } //CXCCLI_SAT
        public DbSet<cxcdir> Cxcdirs { get; set; } //CXCDIR
        public DbSet<cxcfpg> Cxcfpg { get; set; } //CXCFPG
        public DbSet<cfrcli> Cfrcli { get; set; } //CFRCLI

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Añade configuraciones generales aquí
            modelBuilder.Entity<tcausr>(entity =>
            {
                entity.HasNoKey().ToView("tcausr");

                entity.Property(e => e.cia_ventas).HasMaxLength(3).IsUnicode(false).IsFixedLength().HasColumnName("cia_ventas");
                entity.Property(e => e.nom_cto).HasMaxLength(3).IsUnicode(false).IsFixedLength().HasColumnName("nom_cto");
                entity.Property(e => e.nombre).HasMaxLength(10).IsUnicode(false).IsFixedLength().HasColumnName("nombre");
                entity.Property(e => e.nombre_lar).HasMaxLength(40).IsUnicode(false).IsFixedLength().HasColumnName("nombre_lar");
                entity.Property(e => e.puesto).HasMaxLength(15).IsUnicode(false).IsFixedLength().HasColumnName("puesto");
                entity.Property(e => e.pwd).HasMaxLength(10).IsUnicode(false).IsFixedLength().HasColumnName("pwd");
            });

            modelBuilder.Entity<ccecpo>(entity =>
            {
                entity.HasNoKey().ToView("ccecpo");
            });

            modelBuilder.Entity<comestados>(entity =>
            {
                entity.HasNoKey().ToView("comestados");
            });

            // Añade configuraciones específicas para otras entidades aquí

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ServidorACADbContext : BaseDbContext
    {
        

        public ServidorACADbContext(DbContextOptions<ServidorACADbContext> options) : base(options)
        {
        }
    }

    public class ServidorALVDbContext : BaseDbContext
    {
        

        public ServidorALVDbContext(DbContextOptions<ServidorALVDbContext> options) : base(options)
        {
        }
    }
} 
