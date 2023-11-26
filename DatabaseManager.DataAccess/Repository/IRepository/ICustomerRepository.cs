using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Customer? Update(Customer customer);
    }
}
