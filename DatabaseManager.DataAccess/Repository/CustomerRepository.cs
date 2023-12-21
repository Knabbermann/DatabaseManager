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
            if (cCustomer.HasGcRecord && !uCustomer.HasGcRecord)
            {
                webDbContextShard1.Customers.Remove(uCustomer);
                webDbContextShard2.Customers.Add(cCustomer);
                webDbContextShard1.SaveChanges();
                webDbContextShard2.SaveChanges();
                return cCustomer;
            }
            if (!cCustomer.HasGcRecord && uCustomer.HasGcRecord)
            {
                webDbContextShard2.Customers.Remove(uCustomer);
                webDbContextShard1.Customers.Add(cCustomer);
                webDbContextShard1.SaveChanges();
                webDbContextShard2.SaveChanges();
                return cCustomer;
            }

            var config = new MapperConfiguration(x => x.CreateMap<Customer, Customer>());
            var mapper = config.CreateMapper();
            mapper.Map(cCustomer, uCustomer);

            if(uCustomer.HasGcRecord) webDbContextShard2.Update(uCustomer);
            else webDbContextShard1.Update(uCustomer);
            return uCustomer;
        }
    }
}
