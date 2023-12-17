using DatabaseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManager.DataAccess.DbContext
{
    public class WebDbContextShard1 : Microsoft.EntityFrameworkCore.DbContext
    {
        public WebDbContextShard1(DbContextOptions<WebDbContextShard1> options)
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
