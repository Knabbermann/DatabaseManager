using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatabaseManager.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebDbContextShard1 _webDbContextShard1;
        private readonly WebDbContextShard2 _webDbContextShard2;
        private readonly IConfiguration _configuration;

        public UnitOfWork(WebDbContextShard1 webDbContextShard1, WebDbContextShard2 webDbContextShard2, IConfiguration configuration)
        {
            _webDbContextShard1 = webDbContextShard1;
            _webDbContextShard2 = webDbContextShard2;
            _configuration = configuration;

            Customer = new CustomerRepository(_webDbContextShard1, _webDbContextShard2, this);
            Order = new OrderRepository(_webDbContextShard1, _webDbContextShard2, this);
            Payment = new PaymentRepository(_webDbContextShard1, _webDbContextShard2, this);
            Product = new ProductRepository(_webDbContextShard1, _webDbContextShard2, this);
            Review = new ReviewRepository(_webDbContextShard1, _webDbContextShard2, this);
            LogWithId = new LogWithIdRepository(_webDbContextShard1, this);
            LogWithGuid = new LogWithGuidRepository(_webDbContextShard1, this);
            Performance = new PerformanceRepository(_webDbContextShard1, this);
        }

        public ICustomerRepository Customer { get; }
        public IOrderRepository Order { get; }
        public IPaymentRepository Payment { get; }
        public IProductRepository Product { get; }
        public IReviewRepository Review { get; }
        public ILogWithIdRepository LogWithId { get; }
        public ILogWithGuidRepository LogWithGuid { get; }
        public IPerformanceRepository Performance { get; }

        public void SaveChanges(int shardId = 1)
        {
            if(shardId == 1) _webDbContextShard1.SaveChanges();
            else if (shardId == 2) _webDbContextShard2.SaveChanges();
            else throw new ArgumentException("ShardId not known.");
        }

        public IDbConnection GetDbConnection(int shardId = 1)
        {
            var connectionString = _configuration.GetConnectionString("WebDbContextConnectionShard"+shardId);
            return new SqlConnection(connectionString);
        }
    }
}
