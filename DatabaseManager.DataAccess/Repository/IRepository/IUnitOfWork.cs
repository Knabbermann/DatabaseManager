using System.Data;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customer { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        IPaymentRepository Payment { get; }
        IProductRepository Product { get; }
        IReviewRepository Review { get; }
        void SaveChanges();
        IDbConnection GetDbConnection();
    }
}
