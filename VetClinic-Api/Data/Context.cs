using Microsoft.EntityFrameworkCore;
using VetClinic.Entities;

namespace VetClinic.Data
{
                                                                          
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Dono> Donos { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dono>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Ignore(d => d.Pets);
                entity.OwnsOne(d => d.Cpf, cpf =>
                {
                    cpf.Property(c => c.Value).HasColumnName("Cpf");
                });
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.Dono)
                      .WithMany()
                      .HasForeignKey(p => p.DonoId);
            });

            modelBuilder.Entity<Veterinario>(entity =>
            {
                entity.HasKey(v => v.Id);
            });

            modelBuilder.Entity<Consulta>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Pet)
                      .WithMany()
                      .HasForeignKey(c => c.PetId);
                entity.HasOne(c => c.Veterinario)
                      .WithMany()
                      .HasForeignKey(c => c.VeterinarioId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
