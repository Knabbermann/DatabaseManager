using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public void Update(OrderItem orderItem);
    }
}
