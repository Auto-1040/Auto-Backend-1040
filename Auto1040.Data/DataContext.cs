using Auto1040.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auto1040.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> UserData { get; set; }
        public DbSet<UserDetails> UserDetailsData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
