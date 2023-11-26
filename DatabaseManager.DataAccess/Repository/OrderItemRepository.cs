using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class OrderItemRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : Repository<OrderItem>(webDbContext, unitOfWork),
        IOrderItemRepository
    {
        public OrderItem? Update(OrderItem cOrderItem)
        {
            var uOrderItem = unitOfWork.OrderItem.GetById(cOrderItem.Id);
            if (uOrderItem == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<OrderItem, OrderItem>());
            var mapper = config.CreateMapper();
            mapper.Map(cOrderItem, uOrderItem);
            webDbContext.Update(uOrderItem);
            return uOrderItem;
        }
    }
}
