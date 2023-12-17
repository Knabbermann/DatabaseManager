using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class CustomerRepository(WebDbContextShard1 webDbContextShard1, WebDbContextShard2 webDbContextShard2, IUnitOfWork unitOfWork) : Repository<Customer>(webDbContextShard1, webDbContextShard2, unitOfWork),
        ICustomerRepository
    {
        public Customer? Update(Customer cCustomer)
        {
            var uCustomer = unitOfWork.Customer.GetById(cCustomer.Id);
            if (uCustomer == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Customer, Customer>());
            var mapper = config.CreateMapper();
            mapper.Map(cCustomer, uCustomer);
            webDbContextShard1.Update(uCustomer);
            return uCustomer;
        }
    }
}
