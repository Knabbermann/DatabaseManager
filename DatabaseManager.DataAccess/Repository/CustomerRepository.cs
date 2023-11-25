using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class CustomerRepository(WebDbContext webDbContext) : Repository<Customer>(webDbContext), ICustomerRepository
    {
        public void Update(Customer customer)
        {
            webDbContext.Update(customer);
        }
    }
}
