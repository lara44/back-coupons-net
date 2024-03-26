using back_coupons.Entities;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Contact>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
