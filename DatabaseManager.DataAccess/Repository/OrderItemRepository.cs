using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly WebDbContext _webDbContext;

        public OrderItemRepository(WebDbContext webDbContext) : base(webDbContext)
        {
            _webDbContext = webDbContext;
        }

        public void Update(OrderItem orderItem)
        {
            _webDbContext.Update(orderItem);
            _webDbContext.SaveChanges();
        }
    }
}
