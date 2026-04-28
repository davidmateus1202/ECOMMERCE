using Microsoft.EntityFrameworkCore;
using ECommerce.Models;

namespace ECommerce.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<StorefrontSection> StorefrontSections { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product table configuration
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Sku)
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.IsNew)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StorefrontSection>()
                .HasKey(section => section.StorefrontSectionId);

            modelBuilder.Entity<StorefrontSection>()
                .HasIndex(section => section.Slug)
                .IsUnique();

            modelBuilder.Entity<StorefrontSection>()
                .Property(section => section.Slug)
                .IsRequired();

            modelBuilder.Entity<StorefrontSection>()
                .Property(section => section.Category)
                .IsRequired();

            modelBuilder.Entity<StorefrontSection>()
                .Property(section => section.Title)
                .IsRequired();

            modelBuilder.Entity<StorefrontSection>()
                .Property(section => section.Description)
                .IsRequired();

            // User table configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();
        }
    }
}
