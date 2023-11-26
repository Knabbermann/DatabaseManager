using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Order? Update(Order cOrder);
    }
}
