using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderItemRepository(WebDbContext webDbContext) : Repository<OrderItem>(webDbContext),
        IOrderItemRepository
    {
        public void Update(OrderItem orderItem)
        {
            webDbContext.Update(orderItem);
        }
    }
}
