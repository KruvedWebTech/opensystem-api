using Microsoft.EntityFrameworkCore;
using opensystem_api.Models;


namespace opensystem_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Address> Products { get; set; }
        
    }

}
