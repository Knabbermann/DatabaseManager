using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderRepository(WebDbContextShard1 webDbContextShard1,WebDbContextShard2 webDbContextShard2, IUnitOfWork unitOfWork) : Repository<Order>(webDbContextShard1, webDbContextShard2, unitOfWork),
        IOrderRepository
    {
        public Order? Update(Order cOrder)
        {
            var uOrder = unitOfWork.Order.GetById(cOrder.Id);
            if (uOrder == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Order, Order>());
            var mapper = config.CreateMapper();
            mapper.Map(cOrder, uOrder);
            webDbContextShard1.Update(uOrder);
            return uOrder;
        }
    }
}
