using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderRepository(WebDbContext webDbContext) : Repository<Order>(webDbContext), IOrderRepository
    {
        public void Update(Order order)
        {
            webDbContext.Update(order);
        }
    }
}
