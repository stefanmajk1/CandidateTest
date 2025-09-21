using CandidateTest.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTest.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<ProductEntity> Products { get; set; } = null!;
        public DbSet<CategoryEntity> Categories { get; set; } = null!;
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Products
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.ProductName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Description)
                      .HasMaxLength(1000);

                entity.Property(p => p.StockQuantity)
                      .IsRequired();

                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // Categories
            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.CategoryId);

                entity.Property(c => c.CategoryName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(c => c.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // (many-to-many junction)
            modelBuilder.Entity<ProductCategoryEntity>(entity =>
            {
                entity.ToTable("ProductCategories");
                entity.HasKey(pc => new { pc.ProductId, pc.CategoryId });

                entity.HasOne(pc => pc.Product)
                      .WithMany(p => p.ProductCategories)
                      .HasForeignKey(pc => pc.ProductId);

                entity.HasOne(pc => pc.Category)
                      .WithMany(c => c.ProductCategories)
                      .HasForeignKey(pc => pc.CategoryId);
            });
        }
    }
}
