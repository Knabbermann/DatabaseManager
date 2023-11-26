using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public Payment? Update(Payment cPayment);
    }
}
