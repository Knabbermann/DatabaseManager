using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class PaymentRepository(WebDbContext webDbContext) : Repository<Payment>(webDbContext), IPaymentRepository
    {
        public void Update(Payment payment)
        {
            webDbContext.Update(payment);
        }
    }
}
