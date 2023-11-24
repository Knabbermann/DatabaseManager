using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public void Update(Payment payment);
    }
}
