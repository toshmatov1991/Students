using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Students;

public partial class StudContext : DbContext
{
    public StudContext()
    {
    }

    public StudContext(DbContextOptions<StudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fakulity> Fakulities { get; set; }

    public virtual DbSet<Gruppa> Gruppas { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Filename=stud.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fakulity>(entity =>
        {
            entity.ToTable("Fakulity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameFaculity).HasColumnName("nameFaculity");
        });

        modelBuilder.Entity<Gruppa>(entity =>
        {
            entity.ToTable("Gruppa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NumberGroup).HasColumnName("numberGroup");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.FkIdFakulity).HasColumnName("fk_id_fakulity");
            entity.Property(e => e.FkIdGroup).HasColumnName("fk_id_group");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.FkIdFakulityNavigation).WithMany(p => p.Students).HasForeignKey(d => d.FkIdFakulity);

            entity.HasOne(d => d.FkIdGroupNavigation).WithMany(p => p.Students).HasForeignKey(d => d.FkIdGroup);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
