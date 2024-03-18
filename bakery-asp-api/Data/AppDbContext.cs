using bakery_asp_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bakery_asp_api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ProductConfigure(modelBuilder.Entity<Product>());
            CategoryConfigure(modelBuilder.Entity<Category>());
        }

        public void ProductConfigure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.Calories).IsRequired();
        }
        public void CategoryConfigure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(30);
        }
    }
}
