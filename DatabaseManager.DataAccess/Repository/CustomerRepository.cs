using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly WebDbContext _webDbContext;

        public CustomerRepository(WebDbContext webDbContext) : base(webDbContext)
        {
            _webDbContext = webDbContext;
        }

        public void Update(Customer customer)
        {
            _webDbContext.Update(customer);
            _webDbContext.SaveChanges();
        }
    }
}
