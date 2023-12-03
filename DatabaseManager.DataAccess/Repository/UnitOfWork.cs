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

            Customer = new CustomerRepository(_webDbContext, this);
            Order = new OrderRepository(_webDbContext, this);
            Payment = new PaymentRepository(_webDbContext, this);
            Product = new ProductRepository(_webDbContext, this);
            Review = new ReviewRepository(_webDbContext, this);
            LogWithId = new LogWithIdRepository(_webDbContext, this);
            LogWithGuid = new LogWithGuidRepository(_webDbContext, this);
        }

        public ICustomerRepository Customer { get; }
        public IOrderRepository Order { get; }
        public IPaymentRepository Payment { get; }
        public IProductRepository Product { get; }
        public IReviewRepository Review { get; }
        public ILogWithIdRepository LogWithId { get; }
        public ILogWithGuidRepository LogWithGuid { get; }

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
