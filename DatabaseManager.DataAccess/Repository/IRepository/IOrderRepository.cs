using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        public void Update(Order order);
    }
}
