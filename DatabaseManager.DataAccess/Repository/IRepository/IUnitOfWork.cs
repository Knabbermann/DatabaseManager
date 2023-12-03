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
        void SaveChanges();
        IDbConnection GetDbConnection();
    }
}
