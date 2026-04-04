using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leverantör> Leverantörer => Set<Leverantör>();
        public DbSet<Råvara> Råvaror => Set<Råvara>();
        public DbSet<Produkt> Produkter => Set<Produkt>();
        public DbSet<Recept> Recept => Set<Recept>();
        public DbSet<ReceptRad> ReceptRader => Set<ReceptRad>();
        public DbSet<Lager> Lager => Set<Lager>();
        public DbSet<Inköp> Inköp => Set<Inköp>();
        public DbSet<InköpsRad> InköpsRader => Set<InköpsRad>();
        public DbSet<LeverantörRåvara> LeverantörRåvaror => Set<LeverantörRåvara>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Leverantör>().ToTable("leverantorer");
            model.Entity<Råvara>().ToTable("ravaror");
            model.Entity<Produkt>().ToTable("produkter");
            model.Entity<Recept>().ToTable("recept");
            model.Entity<ReceptRad>().ToTable("recept_rader");
            model.Entity<Lager>().ToTable("lager");
            model.Entity<Inköp>().ToTable("inkop");
            model.Entity<InköpsRad>().ToTable("inkops_rader");
            model.Entity<LeverantörRåvara>().ToTable("leverantor_ravara");

            model.Entity<Lager>().HasKey(l => l.RåvaraId);

            model.Entity<ReceptRad>()
                .HasKey(r => new { r.ReceptId, r.RåvaraId });

            model.Entity<InköpsRad>()
                .HasKey(r => new { r.InköpId, r.RåvaraId });

            model.Entity<LeverantörRåvara>()
                .HasKey(lr => new { lr.LeverantörId, lr.RåvaraId });

            model.Entity<Råvara>()
                .HasOne(r => r.Lager)
                .WithOne(l => l.Råvara)
                .HasForeignKey<Lager>(l => l.RåvaraId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Råvara>()
                .HasMany(r => r.ReceptRader)
                .WithOne(rr => rr.Råvara)
                .HasForeignKey(rr => rr.RåvaraId);

            model.Entity<Råvara>()
                .HasMany(r => r.InköpsRader)
                .WithOne(i => i.Råvara)
                .HasForeignKey(i => i.RåvaraId);

        

            model.Entity<Leverantör>()
                .HasMany(l => l.Inköp)
                .WithOne(i => i.Leverantör)
                .HasForeignKey(i => i.LeverantörId);

            model.Entity<LeverantörRåvara>()
                .HasOne(lr => lr.Leverantör)
                .WithMany(l => l.LeverantörRåvaror)
                .HasForeignKey(lr => lr.LeverantörId);

            model.Entity<LeverantörRåvara>()
                .HasOne(lr => lr.Råvara)
                .WithMany(r => r.LeverantörRåvaror)
                .HasForeignKey(lr => lr.RåvaraId);
        }
    }
}