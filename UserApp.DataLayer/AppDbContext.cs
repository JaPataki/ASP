using Microsoft.EntityFrameworkCore;
using UserApp.DataLayer.Entities;

namespace UserApp.DataLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\UserApp.DataLayer"));
                var dbPath = Path.Combine(basePath, "app_database.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Id);

            modelBuilder.Entity<ItemEntity>()
                .HasIndex(i => i.Id);
            
            modelBuilder.Entity<OrderEntity>()
                .HasIndex(o => o.Id);

            modelBuilder.Entity<ItemEntity>()
                .HasIndex(i => i.ItemId);
        }
    }
}
