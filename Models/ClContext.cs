using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppClientesMexaba.Models;

public partial class ClContext : DbContext
{
    public ClContext()
    {
    }

    public ClContext(DbContextOptions<ClContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Vusuario> Vusuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vusuario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vusuarios");

            entity.Property(e => e.cia_ventas)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cia_ventas");
            entity.Property(e => e.nom_cto)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nom_cto");
            entity.Property(e => e.nombre)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nombre");
            entity.Property(e => e.nombre_lar)
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nombre_lar");
            entity.Property(e => e.puesto)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("puesto");
            entity.Property(e => e.pwd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pwd");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
