using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly WebDbContext _webDbContext;

        public PaymentRepository(WebDbContext webDbContext) : base(webDbContext)
        {
            _webDbContext = webDbContext;
        }

        public void Update(Payment payment)
        {
            _webDbContext.Update(payment);
            _webDbContext.SaveChanges();
        }
    }
}
