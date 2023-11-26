using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public OrderItem? Update(OrderItem cOrderItem);
    }
}
