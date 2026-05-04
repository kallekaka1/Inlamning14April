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

        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<SupplierIngredient> SupplierIngredients => Set<SupplierIngredient>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<BakeryProduct> BakeryProducts => Set<BakeryProduct>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table mappings
            modelBuilder.Entity<Supplier>().ToTable("suppliers");
            modelBuilder.Entity<Ingredient>().ToTable("ingredients");
            modelBuilder.Entity<SupplierIngredient>().ToTable("supplier_ingredients");
            modelBuilder.Entity<Customer>().ToTable("customers");
            modelBuilder.Entity<BakeryProduct>().ToTable("bakery_products");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<OrderItem>().ToTable("order_items");

            // SupplierIngredient - Composite Key
            modelBuilder.Entity<SupplierIngredient>()
                .HasKey(si => new { si.SupplierId, si.IngredientId });

            // SupplierIngredient relationships
            modelBuilder.Entity<SupplierIngredient>()
                .HasOne(si => si.Supplier)
                .WithMany(s => s.SupplierIngredients)
                .HasForeignKey(si => si.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierIngredient>()
                .HasOne(si => si.Ingredient)
                .WithMany(i => i.SupplierIngredients)
                .HasForeignKey(si => si.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem - Composite Key
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.BakeryProductId });

            // OrderItem relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.BakeryProduct)
                .WithMany(bp => bp.OrderItems)
                .HasForeignKey(oi => oi.BakeryProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}