using DatabaseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManager.DataAccess.DbContext
{
    public class WebDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public WebDbContext(DbContextOptions<WebDbContext> options)
            : base(options)
        {
        }
        // Define DbSets for your entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<LogWithId> LogWithIds { get; set; }
        public DbSet<LogWithGuid> LogWithGuids { get; set; }
        public DbSet<Performance> Performances { get; set; }
    }
}
