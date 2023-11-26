using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : Repository<Order>(webDbContext, unitOfWork),
        IOrderRepository
    {
        public Order? Update(Order cOrder)
        {
            var uOrder = unitOfWork.Order.GetById(cOrder.Id);
            if (uOrder == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Order, Order>());
            var mapper = config.CreateMapper();
            mapper.Map(cOrder, uOrder);
            webDbContext.Update(uOrder);
            return uOrder;
        }
    }
}
