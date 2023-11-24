using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly WebDbContext _webDbContext;

        public OrderRepository(WebDbContext webDbContext) : base(webDbContext)
        {
            _webDbContext = webDbContext;
        }

        public void Update(Order order)
        {
            _webDbContext.Update(order);
            _webDbContext.SaveChanges();
        }
    }
}
