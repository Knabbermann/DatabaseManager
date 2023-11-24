using DatabaseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManager.DataAccess.DbContext
{
    public class WebDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public WebDbContext(
                DbContextOptions<WebDbContext> options, 
                DbSet<Customer> customers, 
                DbSet<Order> orders, 
                DbSet<OrderItem> orderItems, 
                DbSet<Payment> payments, 
                DbSet<Product> products, 
                DbSet<Review> reviews)
            : base(options)
        {
            Customers = customers;
            Orders = orders;
            OrderItems = orderItems;
            Payments = payments;
            Products = products;
            Reviews = reviews;
        }
        // Define DbSets for your entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
