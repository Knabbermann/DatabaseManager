using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class CustomerRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : Repository<Customer>(webDbContext),
        ICustomerRepository
    {
        public Customer? Update(Customer cCustomer)
        {
            var uCustomer = unitOfWork.Customer.GetById(cCustomer.Id);
            if (uCustomer == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Customer, Customer>());
            var mapper = config.CreateMapper();
            mapper.Map(cCustomer, uCustomer);
            webDbContext.Update(uCustomer);
            return uCustomer;
        }
    }
}
