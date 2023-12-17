using System.Data;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customer { get; }
        IOrderRepository Order { get; }
        IPaymentRepository Payment { get; }
        IProductRepository Product { get; }
        IReviewRepository Review { get; }
        ILogWithIdRepository LogWithId { get; }
        ILogWithGuidRepository LogWithGuid { get; }
        IPerformanceRepository Performance { get; }
        void SaveChanges(int shardId = 1);
        IDbConnection GetDbConnection(int shardId = 1);
    }
}
