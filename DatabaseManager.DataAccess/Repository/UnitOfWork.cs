using System.Data;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DatabaseManager.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebDbContext _webDbContext;
        private readonly IConfiguration _configuration;

        public UnitOfWork(WebDbContext webDbContext, IConfiguration configuration)
        {
            _webDbContext = webDbContext;
            _configuration = configuration;

            Customer = new CustomerRepository(_webDbContext);
            Order = new OrderRepository(_webDbContext);
            OrderItem = new OrderItemRepository(_webDbContext);
            Payment = new PaymentRepository(_webDbContext);
            Product = new ProductRepository(_webDbContext);
            Review = new ReviewRepository(_webDbContext);
        }

        public ICustomerRepository Customer { get; }
        public IOrderRepository Order { get; }
        public IOrderItemRepository OrderItem { get; }
        public IPaymentRepository Payment { get; }
        public IProductRepository Product { get; }
        public IReviewRepository Review { get; }

        public void SaveChanges()
        {
            _webDbContext.SaveChanges();
        }

        public IDbConnection GetDbConnection()
        {
            var connectionString = _configuration.GetConnectionString("WebDbContextConnection");
            return new SqlConnection(connectionString);
        }
    }
}
